using CommitHelper.Domain.MessageGeneration.Repository;
using CommitHelper.Infra.Adapters;

namespace CommitHelper.Infra.Repositories.AI;

public class AiCommitMessageRepository(IAiAdapter adapter) : IAICommitMessageRepository
{
    public async Task<string> GenerateMessageAsync(string fullPrompt, CancellationToken ct = default)
    {
        var rawResponse = await adapter.GenerateContentAsync(fullPrompt, ct);
        return rawResponse.Trim();
    }
}
