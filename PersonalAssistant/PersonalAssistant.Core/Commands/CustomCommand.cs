namespace PersonalAssistant.Core.Commands;

public class CustomCommand : BaseCommand
{
    public CustomCommand(string command, string action) : base(command, action)
    {
    }
    
    public override async Task Execute()
    {
        Console.WriteLine("Custom");
    }
}