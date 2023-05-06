using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using PersonalAssistant.Core.AssistantModels;
using PersonalAssistant.Core.Enums;
using Vosk;

namespace PersonalAssistant.Core;

public static class Startup
{
    public static void ConfigureAssistant(this IServiceCollection services, Action<AssistantOptions> setupOptions)
    {
        var options = new AssistantOptions();
        setupOptions(options);
        
        var languageModel = new Model(options.LanguageModelPath);
        services.AddSingleton(languageModel);

        var openAiService = new OpenAIService(new OpenAiOptions { ApiKey = options.OpenAiKey });
        services.AddSingleton<IOpenAIService>(openAiService);

        var aiQuestionAssistant = new AiQuestionAssistant(openAiService);
        services.AddSingleton(aiQuestionAssistant);

        var waveIn = new WaveInEvent
        {
            DeviceNumber = 0, // indicates which microphone to use
            WaveFormat = new WaveFormat(rate: 16000, bits: 16, channels: 1),
            BufferMilliseconds = 20
        };
        services.AddSingleton<IWaveIn>(waveIn);

        var voiceRecognizer = new VoiceRecognizer(languageModel, waveIn);
        services.AddSingleton(voiceRecognizer);

        var commandHandler = new CommandHandler(options.WakeUpCommand, aiQuestionAssistant, options.ThresholdRecognizeCommandPercent);
        services.AddSingleton(commandHandler);

        services.AddSingleton(new Assistant(voiceRecognizer, commandHandler, options.CommandHandleType));
    }
}