using Microsoft.Extensions.Hosting;
using PersonalAssistant.Core;

namespace PersonalAssistant.Tests;

public class StartupConsole : IHostedService
{
    private readonly VoiceRecognizer _voiceRecognizer;
    private readonly CommandHandler _commandHandler;

    public StartupConsole(VoiceRecognizer voiceRecognizer, CommandHandler commandHandler)
    {
        _voiceRecognizer = voiceRecognizer;
        _commandHandler = commandHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _commandHandler.LoadCommands(null);
        
        _voiceRecognizer.Init();
        _voiceRecognizer.StartListen();
        _voiceRecognizer.VoiceRecognized += OnVoiceRecognized;
        
        return Task.CompletedTask;
    }

    private async void OnVoiceRecognized(object sender, string text)
    {
        var ask = await _commandHandler.HandleCommand(text, true);
        Console.WriteLine(ask);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _voiceRecognizer.StopListen();
    }
}