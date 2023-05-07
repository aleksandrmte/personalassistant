namespace PersonalAssistant.Core.Commands;

public abstract class BaseCommand
{
    protected BaseCommand(string command, string action)
    {
        Command = command?.ToLower();
        Action = action;
    }
    public string Command { get; }
    public string Action { get; }
    public abstract Task<CommandResult> Execute();
}