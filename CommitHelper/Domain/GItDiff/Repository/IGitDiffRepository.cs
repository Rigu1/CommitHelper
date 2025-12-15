namespace CommitHelper.Domain.GitDiff;

public interface IGitDiffRepository
{
    Task<GitDiff> GetAsync(CancellationToken ct = default);
}
