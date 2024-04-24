using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using McMaster.Extensions.CommandLineUtils;
using NpDownloader.Extensions;
using NpDownloader.Models;
using ShellProgressBar;
using static System.Threading.Tasks.Task;

namespace NpDownloader.Commands;

public class DownloadPkgFilesCommand : AbstractCommand
{
    private const string SceNpUrl = "https://a0.ww.np.dl.playstation.net/tpl/np/{0}/{0}-ver.xml";

    public DownloadPkgFilesCommand() : base("download", "Download all patches for the given Title ID")
    {
    }

    public override void Configure(CommandLineApplication cmd)
    {
        cmd.AddArgument(new CommandArgument
        {
            Name = "Title ID",
            Description = "A comma-separated list of SCE title IDs",
        });

        cmd.AddOption(new CommandOption("-o|--outputdir <dir>", CommandOptionType.SingleOrNoValue)
        {
            Description = "The output folder to download patch files. If ending with '/' no titleId folders will be created",
        });
        cmd.HelpOption();
    }

    public override async Task<int> Execute(CommandLineApplication cmd)
    {
        var npid = cmd.GetArgument("Title ID")?.Value?.Split(',') ?? Array.Empty<string>();

        if (npid.Length < 1)
        {
            throw new ArgumentException("TitleID must be specified!");
        }

        var masterBar = new ProgressBar(npid.Length, $"Getting patches for {string.Join(',', npid)}");
        
        await Parallel.ForEachAsync(npid, async (tid, token) =>
        {
            // Custom handler to ignore ssl certificate validation
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            HttpClient client = new HttpClient(handler);
            using var response = await client.GetAsync(string.Format(SceNpUrl, tid.Trim()), token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync(token));
            }

            Console.WriteLine(response.Content.ReadAsStringAsync(token).Result);

            var serializer = new XmlSerializer(typeof(TitlePatch));

            if (serializer.Deserialize(await response.Content.ReadAsStreamAsync(token)) is not TitlePatch titlePatch)
            {
                return;
            }

            var pbar = masterBar.Spawn(titlePatch.Tag?.Package.Count ?? 0,
                $"Downloading {titlePatch.Tag?.Package.Count ?? 0} patch files for {titlePatch.TitleId}...", new ProgressBarOptions
                {
                    DisableBottomPercentage = true,
                    PercentageFormat = "",
                });
            var saveTasks = new List<Task>();
            foreach (var pkg in titlePatch.Tag?.Package ?? new List<Package>())
            {
                var outDir = cmd.GetOption("outputdir")?.Value() ?? $"./{titlePatch.TitleId}";

                // Handle case where output directory is specified with trailing slash which designates that files 
                // should all be dropped within the same folder
                if (!cmd.GetOption("outputdir")?.Value()?.EndsWith('/') ?? false)
                {
                    outDir = Path.Combine(outDir, titlePatch.TitleId);
                }
                
                Directory.CreateDirectory(outDir);
                saveTasks.Add(Run(async () =>
                {
                    var filePath = Path.Combine(outDir, pkg.Url.Split('/').Last());
                    using var dlTaskbar =
                        pbar.SpawnIndeterminate($"Downloading PKG version {pkg.Version} to path {filePath}",
                            new ProgressBarOptions
                            {
                                ProgressCharacter = '─',
                                CollapseWhenFinished = true,
                            });

                    await using var pkgFile = await client.GetStreamAsync(pkg.Url, token);
                    await using var file = File.Create(filePath);
                    await pkgFile.CopyToAsync(file, token);
                    dlTaskbar.Finished();
                }, token));
            }

            Console.WriteLine("Saving files...");
            WaitAll(saveTasks.ToArray(), token);
            pbar.Message += "Complete!";
            pbar.Dispose();
            masterBar.Tick();
        });

        masterBar.Dispose();
        return 0;
    }
}