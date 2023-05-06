using PersonalAssistant.Core.Enums;

namespace PersonalAssistant.Core.AssistantModels;

public class AssistantOptions
{
    public string LanguageModelPath { get; set; }
    public string OpenAiKey { get; set; }
    public string WakeUpCommand { get; set; }
    public double ThresholdRecognizeCommandPercent { get; set; }
    public CommandHandleType CommandHandleType { get; set; }
}