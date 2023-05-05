using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace PersonalAssistant.Core;

public class AiQuestionAssistant
{
    private readonly IOpenAIService _openAiService;

    public AiQuestionAssistant(IOpenAIService openAiService)
    {
        _openAiService = openAiService;
    }

    public async Task<string> AskQuestion(string question, int? askLettersCount)
    {
        if (string.IsNullOrEmpty(question))
        {
            throw new InvalidDataException("Question is required.");
        }

        var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromUser(question)
            },
            Model = Models.ChatGpt3_5Turbo,
            MaxTokens = askLettersCount
        });

        return completionResult.Successful ? completionResult.Choices.First().Message.Content : "Error";
    }
}