namespace PersonalAssistant.Core.Commands;

public abstract class BaseCommand
{
    public string Command { get; set; }
    public string Action { get; set; }
    public abstract Task Execute();
}