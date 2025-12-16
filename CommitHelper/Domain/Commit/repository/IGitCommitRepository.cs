namespace CommitHelper.Domain.Commit.repository;

public interface IGitCommitRepository
{
    Task CommitAsync(string message);
}
