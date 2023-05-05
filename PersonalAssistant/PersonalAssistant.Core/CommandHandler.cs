using PersonalAssistant.Core.Commands;

namespace PersonalAssistant.Core;

public class CommandHandler
{
    private readonly string _wakeUpCommand;
    private readonly AiQuestionAssistant _aiQuestionAssistant;
    private List<BaseCommand> _commands;

    public CommandHandler(string wakeUpCommand, AiQuestionAssistant aiQuestionAssistant)
    {
        _wakeUpCommand = wakeUpCommand;
        _aiQuestionAssistant = aiQuestionAssistant;
        _commands = new List<BaseCommand>();
    }

    public void LoadCommands(IEnumerable<BaseCommand> commands)
    {
        _commands = SystemCommands.GetCommands();
        
        commands = commands?.ToList() ?? new List<BaseCommand>();

        _commands.AddRange(commands);
    }
    
    public async Task<string> HandleCommand(string commandText, bool useAi)
    {
        if (string.IsNullOrEmpty(commandText))
        {
            return string.Empty;
        }

        commandText = commandText.Trim();
        
        if (!commandText.StartsWith(_wakeUpCommand))
        {
            return string.Empty;
        }

        commandText = commandText.Replace(_wakeUpCommand, string.Empty).Trim();

        var command = _commands.FirstOrDefault(x => x.Command == commandText);

        if (command == null)
        {
            return useAi ? await _aiQuestionAssistant.AskQuestion(commandText, 50) : string.Empty;
        }
        
        await command.Execute();
        return string.Empty;

    }
}