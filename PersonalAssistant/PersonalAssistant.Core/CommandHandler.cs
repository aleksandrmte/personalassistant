using PersonalAssistant.Core.Commands;
using PersonalAssistant.Core.Enums;

namespace PersonalAssistant.Core;

public class CommandHandler
{
    private readonly string _wakeUpCommand;
    private readonly AiQuestionAssistant _aiQuestionAssistant;
    private readonly CommandRecognizer _commandRecognizer;
    public event EventHandler<Task> BeforeExecuteCommand;
    public event EventHandler<Task> BeforeSearchAi;

    public CommandHandler(string wakeUpCommand, AiQuestionAssistant aiQuestionAssistant, double thresholdRecognizeCommandPercent)
    {
        _wakeUpCommand = wakeUpCommand.ToLower();
        _aiQuestionAssistant = aiQuestionAssistant;
        _commandRecognizer = new CommandRecognizer(SystemCommands.GetCommands(), thresholdRecognizeCommandPercent);
    }
    
    public void LoadCommands(IEnumerable<BaseCommand> commands)
    {
        _commandRecognizer.AddCommands(commands);
    }
    
    public async Task<string> HandleCommand(string commandText, CommandHandleType handleType)
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

        if (handleType is CommandHandleType.UseAll or CommandHandleType.UseOnlyCommands)
        {
            var command = _commandRecognizer.GetCommand(commandText);

            if (command != null)
            {
                BeforeExecuteCommand?.Invoke(this, Task.CompletedTask);
                
                await command.Execute();
            }
        }

        if (handleType is CommandHandleType.UseOnlyCommands)
            return string.Empty;

        BeforeSearchAi?.Invoke(this, Task.CompletedTask);
        
        return await _aiQuestionAssistant.AskQuestion(commandText, 100);
    }
}