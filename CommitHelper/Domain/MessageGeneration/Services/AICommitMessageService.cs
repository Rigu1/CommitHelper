using CommitHelper.Configuration;
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
            throw new InvalidOperationException("커밋 메시지 생성에 필요한 Git Diff 내용이 없습니다.");
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
