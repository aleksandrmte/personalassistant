using PersonalAssistant.Core.Commands;
using PersonalAssistant.Core.Commands.System;

namespace PersonalAssistant.Core;

public static class SystemCommands
{
    public static List<BaseCommand> GetCommands() => new()
    {
        new YoutubeCommand("Открой ютюб", "https://www.youtube.com")
    };
}