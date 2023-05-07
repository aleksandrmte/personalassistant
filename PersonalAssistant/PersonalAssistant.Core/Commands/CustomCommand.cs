namespace PersonalAssistant.Core.Commands;

public class CustomCommand : BaseCommand
{
    public CustomCommand(string command, string action) : base(command, action)
    {
    }
    
    public override async Task<CommandResult> Execute()
    {
        Console.WriteLine("Custom");

        return await Task.FromResult(CommandResult.Ok());
    }
}