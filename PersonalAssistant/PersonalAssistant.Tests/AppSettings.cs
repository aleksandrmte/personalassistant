using PersonalAssistant.Core.Commands;
using PersonalAssistant.Core.Enums;

namespace PersonalAssistant.Tests;

public class AppSettings
{
    public string LanguageModelPath { get; set; }
    public string OpenAiKey { get; set; }
    public string WakeUpCommand { get; set; }
    public double ThresholdRecognizeCommandPercent { get; set; }
    public CommandHandleType CommandHandleType { get; set; }
    public List<CustomActionTemplate> CustomCommands { get; set; }
}