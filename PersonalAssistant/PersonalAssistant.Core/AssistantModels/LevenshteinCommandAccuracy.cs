namespace PersonalAssistant.Core.AssistantModels;

public class LevenshteinCommandAccuracy
{
    public string Command { get; set; }
    public double AccuracyPercent { get; set; }

    public static LevenshteinCommandAccuracy Create(string command, double accuracyPercent)
    {
        return new LevenshteinCommandAccuracy
        {
            Command = command,
            AccuracyPercent = accuracyPercent
        };
    }
}