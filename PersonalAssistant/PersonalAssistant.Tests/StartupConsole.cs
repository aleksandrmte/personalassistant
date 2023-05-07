using System.Speech.Synthesis;
using Microsoft.Extensions.Hosting;
using NAudio.Wave;
using PersonalAssistant.Core;
using PersonalAssistant.Core.Commands;

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
        _assistant.AfterExecuteCommand += AssistantOnAfterExecuteCommand;
        
        return Task.CompletedTask;
    }

    private static void AssistantOnAfterExecuteCommand(object sender, CommandResult e)
    {
        if (!string.IsNullOrEmpty(e.SayCommand))
        {
            PlayVoice(e.SayCommand);
        }
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
        PlayVoice(text);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _assistant.VoiceRecognized -= OnVoiceRecognized;
        _assistant.BeforeExecuteCommand -= AssistantOnBeforeExecuteCommand;
        _assistant.BeforeSearchAi -= AssistantOnBeforeSearchAi;
        
        await _assistant.StopAsync();
    }

    private static void PlayVoice(string text)
    {
        var voice = new SpeechSynthesizer();
        voice.SetOutputToDefaultAudioDevice();
        voice.Rate = 4;
        voice.Speak(text);
    }

    private static void PlaySound(string soundPath)
    {
        using var mf = new MediaFoundationReader(soundPath);
        using var wo = new WasapiOut();
        wo.Init(mf);
        wo.Play();
        while (wo.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(500);
        }
    }
}