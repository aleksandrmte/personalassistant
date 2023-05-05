namespace PersonalAssistant.Core.Commands;

public class CustomCommand : BaseCommand
{
    public override async Task Execute()
    {
        Console.WriteLine("Custom");
    }
}