using PersonalAssistant.Core.Commands.System;

namespace PersonalAssistant.Core.Commands;

public static class SystemCommands
{
    public static List<BaseCommand> GetCommands() => new()
    {
        new YoutubeCommand("Открой ютюб", "https://www.youtube.com"),
        new GetTimeCommand("Сколько сейчас время", string.Empty)
    };
}