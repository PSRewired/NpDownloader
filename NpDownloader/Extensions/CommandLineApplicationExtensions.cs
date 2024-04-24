using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using NpDownloader.Commands;

namespace NpDownloader.Extensions;

public static class CommandLineApplicationExtensions
{

        /// <summary>
        /// Adds a collection of commands to the CLI.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="commands"></param>
        public static void AddRange(this CommandLineApplication cli, IEnumerable<AbstractCommand> commands)
        {
            foreach (var c in commands)
            {
                cli.AddCommand(c);
            }
        }

        /// <summary>
        /// Adds a single command to the CLI.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="command"></param>
        public static void AddCommand(this CommandLineApplication cli, AbstractCommand command)
        {
            cli.Command(command.CommandName, cmd =>
            {
                cmd.Description = command.Description;

                command.Configure(cmd);
                cmd.OnExecuteAsync(async _ => await command.Execute(cmd));
            });
        }

        /// <summary>
        /// Gets argument by argument name.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CommandArgument? GetArgument(this CommandLineApplication cli, string name)
        {
            return cli.Arguments.FirstOrDefault(a => a.Name?.Equals(name) ?? false);
        }

        /// <summary>
        /// Gets options by option name.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CommandOption? GetOption(this CommandLineApplication cli, string name)
        {
            return cli.Options.FirstOrDefault(o => o.LongName?.Equals(name) ?? false);
        }
}