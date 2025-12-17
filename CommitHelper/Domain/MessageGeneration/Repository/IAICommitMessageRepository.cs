namespace CommitHelper.Domain.MessageGeneration.Repository;

public interface IAICommitMessageRepository
{
    Task<string> GenerateMessageAsync(string fullPrompt, CancellationToken ct = default);
}
