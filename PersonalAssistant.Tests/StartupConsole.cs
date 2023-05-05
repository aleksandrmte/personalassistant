using Microsoft.Extensions.Hosting;
using PersonalAssistant.Core;

namespace PersonalAssistant.Tests;

public class StartupConsole : IHostedService
{
    private readonly VoiceRecognizer _voiceRecognizer;
    private readonly AiQuestionAssistant _aiQuestionAssistant;

    public StartupConsole(VoiceRecognizer voiceRecognizer, AiQuestionAssistant aiQuestionAssistant)
    {
        _voiceRecognizer = voiceRecognizer;
        _aiQuestionAssistant = aiQuestionAssistant;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _voiceRecognizer.Init();
        _voiceRecognizer.StartListen();
        _voiceRecognizer.VoiceRecognized += OnVoiceRecognized;
        
        return Task.CompletedTask;
    }

    private async void OnVoiceRecognized(object sender, string text)
    {
        var ask = await _aiQuestionAssistant.AskQuestion(text, 50);
        Console.WriteLine(ask);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _voiceRecognizer.StopListen();
    }
}