using CommitHelper.Domain.Staging.Repository;

namespace CommitHelper.Domain.Staging.Service;

public class GitDiffService(IGitDiffRepository repository)
{
    public async Task<GitDiff> GetStagedDiffAsync(CancellationToken ct = default)
    {
        return await repository.GetAsync(ct);
    }
}
