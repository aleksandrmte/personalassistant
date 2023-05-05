using PersonalAssistant.Core.Commands;

namespace PersonalAssistant.Core;

public class CommandHandler
{
    private readonly string _wakeUpCommand;
    private readonly AiQuestionAssistant _aiQuestionAssistant;
    private readonly List<BaseCommand> _commands;

    public CommandHandler(string wakeUpCommand, AiQuestionAssistant aiQuestionAssistant)
    {
        _wakeUpCommand = wakeUpCommand.ToLower();
        _aiQuestionAssistant = aiQuestionAssistant;
        _commands = SystemCommands.GetCommands();
    }

    public void LoadCommands(IEnumerable<BaseCommand> commands)
    {
        commands = commands?.ToList() ?? new List<BaseCommand>();

        _commands.AddRange(commands);
    }
    
    public async Task<string> HandleCommand(string commandText, bool useAi)
    {
        if (string.IsNullOrEmpty(commandText))
        {
            return string.Empty;
        }

        commandText = commandText.Trim().ToLower();
        
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