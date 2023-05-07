using System.Diagnostics;

namespace PersonalAssistant.Core.Commands.System;

public class YoutubeCommand : BaseCommand
{
    public YoutubeCommand(string command, string action) : base(command, action)
    {
    }
    
    public override async Task<CommandResult> Execute()
    {
        await Task.FromResult(Process.Start("explorer", Action));

        return CommandResult.Ok();
    }
}