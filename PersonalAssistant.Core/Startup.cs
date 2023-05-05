using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using Vosk;

namespace PersonalAssistant.Core;

public static class Startup
{
    public static void CoreConfigure(this IServiceCollection services, string modelPath, string apiKey)
    {
        var model = new Model(modelPath);
        services.AddSingleton(model);
                    
        var openAiService = new OpenAIService(new OpenAiOptions { ApiKey = apiKey });
        services.AddSingleton(openAiService);
                    
        var waveIn = new WaveInEvent
        {
            DeviceNumber = 0, // indicates which microphone to use
            WaveFormat = new WaveFormat(rate: 16000, bits: 16, channels: 1),
            BufferMilliseconds = 20
        };
        services.AddSingleton(waveIn);

        services.AddSingleton<VoiceRecognizer>();
        services.AddSingleton<AiQuestionAssistant>();
    }
}