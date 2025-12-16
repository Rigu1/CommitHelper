namespace CommitHelper.Domain.Commit.Repository;

public interface IGitCommitRepository
{
    Task CommitAsync(string message);
}
