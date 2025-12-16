using CommitHelper.Domain.Commit.repository;

namespace CommitHelper.Domain.Commit.Services;

public class GitCommitService(IGitCommitRepository repository)
{
    public async Task CommitAsync(CommitMessage message)
    {
        await repository.CommitAsync(message.Value);
    }
}
