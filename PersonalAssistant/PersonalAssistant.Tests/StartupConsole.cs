using System.Diagnostics;
using System.Speech.Synthesis;
using Microsoft.Extensions.Hosting;
using PersonalAssistant.Core;

namespace PersonalAssistant.Tests;

public class StartupConsole : IHostedService
{
    private readonly Assistant _assistant;

    public StartupConsole(Assistant assistant)
    {
        _assistant = assistant;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _assistant.LoadCommands(null);
        _assistant.Start();
        
        _assistant.VoiceRecognized += OnVoiceRecognized;
        _assistant.BeforeExecuteCommand += AssistantOnBeforeExecuteCommand;
        _assistant.BeforeSearchAi += AssistantOnBeforeSearchAi;
        
        return Task.CompletedTask;
    }

    private static void AssistantOnBeforeSearchAi(object sender, Task e)
    {
        PlaySound(@"E:\\siri-vot-chto-mne-udalos-naiti.mp3");
    }

    private static void AssistantOnBeforeExecuteCommand(object sender, Task e)
    {
        PlaySound(@"E:\\zvuk-siri-pered-otvetom.mp3");
    }

    private static void OnVoiceRecognized(object sender, string text)
    {
        Console.WriteLine(text);
        
        var voice = new SpeechSynthesizer();
        voice.SetOutputToDefaultAudioDevice();
        voice.Rate = 4;
        voice.Speak(text);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _assistant.StopAsync();
    }

    private static void PlaySound(string soundPath)
    {
        var ps = new ProcessStartInfo(soundPath)
        {
            UseShellExecute = true,
            Verb = "open"
        };
        Process.Start(ps);
    }
}