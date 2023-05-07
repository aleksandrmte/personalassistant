using PersonalAssistant.Core.Commands;
using PersonalAssistant.Core.Enums;

namespace PersonalAssistant.Core;

public class Assistant
{
    private readonly VoiceRecognizer _voiceRecognizer;
    private readonly CommandHandler _commandHandler;
    private readonly CommandHandleType _handleType;
    public event EventHandler<string> VoiceRecognized;
    public event EventHandler<Task> BeforeExecuteCommand;
    public event EventHandler<CommandResult> AfterExecuteCommand; 
    public event EventHandler<Task> BeforeSearchAi;

    public Assistant(VoiceRecognizer voiceRecognizer, CommandHandler commandHandler, CommandHandleType handleType)
    {
        _voiceRecognizer = voiceRecognizer;
        _commandHandler = commandHandler;
        _handleType = handleType;
    }
    
    public void Start()
    {
        _voiceRecognizer.Init();
        _voiceRecognizer.StartListen();
        _voiceRecognizer.VoiceRecognized += OnVoiceRecognized;
        
        _commandHandler.BeforeExecuteCommand += CommandHandlerOnBeforeExecuteCommand;
        _commandHandler.BeforeSearchAi += CommandHandlerOnBeforeSearchAi;
        _commandHandler.AfterExecuteCommand += CommandHandlerOnAfterExecuteCommand;
    }
    
    public void LoadCommands(IEnumerable<BaseCommand> commands)
    {
        _commandHandler.LoadCommands(commands);
    }

    private void CommandHandlerOnBeforeSearchAi(object sender, Task e)
    {
        BeforeSearchAi?.Invoke(this, Task.CompletedTask);
    }

    private void CommandHandlerOnBeforeExecuteCommand(object sender, Task e)
    {
        BeforeExecuteCommand?.Invoke(this, Task.CompletedTask);
    }

    private void CommandHandlerOnAfterExecuteCommand(object sender, CommandResult e)
    {
        AfterExecuteCommand?.Invoke(this, e);
    }

    public async Task StopAsync()
    {
        _commandHandler.BeforeExecuteCommand -= CommandHandlerOnBeforeExecuteCommand;
        _commandHandler.BeforeSearchAi -= CommandHandlerOnBeforeSearchAi;
        _commandHandler.AfterExecuteCommand -= CommandHandlerOnAfterExecuteCommand;
        
        _voiceRecognizer.VoiceRecognized -= OnVoiceRecognized;
        await _voiceRecognizer.StopListen();
    }
    
    private async void OnVoiceRecognized(object sender, string text)
    {
        var ask = await _commandHandler.HandleCommand(text, _handleType);
        
        VoiceRecognized?.Invoke(this, ask);
    }
}