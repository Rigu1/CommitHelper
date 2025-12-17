using CommitHelper.Configuration;
using CommitHelper.Domain.MessageGeneration.Constants;
using CommitHelper.Domain.MessageGeneration.Repository;

namespace CommitHelper.Domain.MessageGeneration.Services;

public class AICommitMessageService(
    IAICommitMessageRepository aiRepository,
    AiSettings aiSettings)
{
    public async Task<GeneratedMessage> GenerateMessageAsync(string diffContent, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(diffContent))
        {
            throw new InvalidOperationException(MessageGenerationConstants.ErrorNoDiffContent);
        }

        var formattedDiff = diffContent.Trim();

        var fullPrompt =
            $"""
             [SYSTEM INSTRUCTION]:
             {aiSettings.SystemPrompt}

             [GIT DIFF CONTENT]:
             {formattedDiff}
             """;

        var rawMessage = await aiRepository.GenerateMessageAsync(fullPrompt, ct);

        return new GeneratedMessage(rawMessage);
    }
}
