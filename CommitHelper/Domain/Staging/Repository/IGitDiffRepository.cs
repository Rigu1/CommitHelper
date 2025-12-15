namespace CommitHelper.Domain.Staging.Repository;

public interface IGitDiffRepository
{
    Task<GitDiff> GetAsync(CancellationToken ct = default);
}
