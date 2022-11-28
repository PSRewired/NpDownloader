using McMaster.Extensions.CommandLineUtils;

namespace NpDownloader.Commands;

public abstract class AbstractCommand
{
    public string CommandName { get; }
    public string Description { get; }

    protected AbstractCommand(string commandName, string description)
    {
        CommandName = commandName;
        Description = description;
    }

    /// <summary>
    /// Allows passing of configuration options for this command.
    /// </summary>
    /// <param name="cmd"></param>
    public abstract void Configure(CommandLineApplication cmd);

    /// <summary>
    /// Called to perform that task for this command.
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public abstract Task<int> Execute(CommandLineApplication cmd); 
}