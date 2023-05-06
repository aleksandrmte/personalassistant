using PersonalAssistant.Core.AssistantModels;
using PersonalAssistant.Core.Commands;

namespace PersonalAssistant.Core;

public class CommandRecognizer
{
    private readonly double _thresholdPercent;
    private List<BaseCommand> _commands;

    public CommandRecognizer(IEnumerable<BaseCommand> commands, double thresholdPercent)
    {
        _thresholdPercent = thresholdPercent;
        _commands = commands?.ToList() ?? new List<BaseCommand>();
    }

    public void AddCommands(IEnumerable<BaseCommand> commands)
    {
        commands = commands?.ToList() ?? new List<BaseCommand>();
        
        _commands.AddRange(commands);

        _commands = _commands.DistinctBy(x => x.Command).ToList();
    }

    public BaseCommand GetCommand(string commandText)
    {
        var possibleCommands = GetPossibleCommands(commandText);
        
        return !possibleCommands.Any() ? null : _commands.FirstOrDefault(x => x.Command == possibleCommands[0].Command);
    }

    private IReadOnlyList<LevenshteinCommandAccuracy> GetPossibleCommands(string commandText)
    {
        var possibleCommands = new List<LevenshteinCommandAccuracy>();
        
        var levenshtein = new Fastenshtein.Levenshtein(commandText);
        
        foreach (var command in _commands.Select(x => x.Command).ToList())
        {
            var levenshteinDistance = levenshtein.DistanceFrom(command);

            var onePercent = 100.0 / command.Length;

            var commandTextAccuracy = 100.0 - onePercent * levenshteinDistance;

            if (commandTextAccuracy >= _thresholdPercent)
            {
                possibleCommands.Add(LevenshteinCommandAccuracy.Create(command, commandTextAccuracy));
            }
        }

        return possibleCommands.OrderByDescending(x => x.AccuracyPercent).ToList();
    }
}