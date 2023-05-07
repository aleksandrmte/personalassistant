namespace PersonalAssistant.Core.Commands;

public class CommandResult
{
    public string SayCommand { get; set; }

    public static CommandResult Ok()
    {
        return new CommandResult();
    }
    
    public static CommandResult Say(string text)
    {
        return new CommandResult
        {
            SayCommand = text
        };
    }
}