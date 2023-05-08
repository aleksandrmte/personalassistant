using System.Diagnostics;
using Newtonsoft.Json;

namespace PersonalAssistant.Core.Commands;

public class CustomCommand : BaseCommand
{
    private readonly List<TerminalCommand> _commands;

    public static CustomCommand Create(CustomActionTemplate action)
    {
        return new CustomCommand(action.Command, JsonConvert.SerializeObject(action.TerminalCommands));
    }
    
    private CustomCommand(string command, string action) : base(command, action)
    {
        if (string.IsNullOrEmpty(action))
            return;

        _commands = JsonConvert.DeserializeObject<List<TerminalCommand>>(action);
    }
    
    public override async Task<CommandResult> Execute()
    {
        foreach (var terminalCommand in _commands)
        {
            Process.Start($"{terminalCommand.Action} {terminalCommand.ExePath} {terminalCommand.ExeArgs}");
        }

        return await Task.FromResult(CommandResult.Ok());
    }
}