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
    public event EventHandler<Task> BeforeSearchAi;

    public Assistant(VoiceRecognizer voiceRecognizer, CommandHandler commandHandler, CommandHandleType handleType)
    {
        _voiceRecognizer = voiceRecognizer;
        _commandHandler = commandHandler;
        _handleType = handleType;
    }

    private void CommandHandlerOnBeforeSearchAi(object sender, Task e)
    {
        BeforeSearchAi?.Invoke(this, Task.CompletedTask);
    }

    private void CommandHandlerOnBeforeExecuteCommand(object sender, Task e)
    {
        BeforeExecuteCommand?.Invoke(this, Task.CompletedTask);
    }

    public void LoadCommands(IEnumerable<BaseCommand> commands)
    {
        _commandHandler.LoadCommands(commands);
    }

    public void Start()
    {
        _voiceRecognizer.Init();
        _voiceRecognizer.StartListen();
        _voiceRecognizer.VoiceRecognized += OnVoiceRecognized;
        
        _commandHandler.BeforeExecuteCommand += CommandHandlerOnBeforeExecuteCommand;
        _commandHandler.BeforeSearchAi += CommandHandlerOnBeforeSearchAi;
    }
    
    public async Task StopAsync()
    {
        _commandHandler.BeforeExecuteCommand -= CommandHandlerOnBeforeExecuteCommand;
        _commandHandler.BeforeSearchAi -= CommandHandlerOnBeforeSearchAi;
        
        _voiceRecognizer.VoiceRecognized -= OnVoiceRecognized;
        await _voiceRecognizer.StopListen();
    }
    
    private async void OnVoiceRecognized(object sender, string text)
    {
        var ask = await _commandHandler.HandleCommand(text, _handleType);
        
        VoiceRecognized?.Invoke(this, ask);
    }
}