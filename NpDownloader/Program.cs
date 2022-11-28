using McMaster.Extensions.CommandLineUtils;
using NpDownloader.Commands;
using NpDownloader.Extensions;

var app = new CommandLineApplication();
app.AddCommand(new DownloadPkgFilesCommand());

app.OnExecute(() =>
{
    app.ShowHelp();
});

return app.Execute(args);
