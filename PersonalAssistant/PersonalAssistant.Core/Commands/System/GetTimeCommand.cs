namespace PersonalAssistant.Core.Commands.System;

public class GetTimeCommand : BaseCommand
{
    public GetTimeCommand(string command, string action) : base(command, action)
    {
    }
    
    public override async Task<CommandResult> Execute()
    {
        return await Task.FromResult(CommandResult.Say($"Сейчас {DateTime.Now.Hour} часов, {DateTime.Now.Minute} минут."));
    }
}