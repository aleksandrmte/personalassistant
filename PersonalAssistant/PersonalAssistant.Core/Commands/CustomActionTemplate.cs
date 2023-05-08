namespace PersonalAssistant.Core.Commands;

public class CustomActionTemplate
{
    public string Command { get; set; }
    public List<TerminalCommand> TerminalCommands { get; set; }
}