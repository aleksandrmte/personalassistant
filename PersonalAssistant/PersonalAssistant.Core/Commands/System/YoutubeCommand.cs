using System.Diagnostics;

namespace PersonalAssistant.Core.Commands.System;

public class YoutubeCommand : BaseCommand
{
    public override async Task Execute()
    {
        await Task.FromResult(Process.Start("explorer", Action));
    }
}