using CommitHelper.Domain.GitDiff;

namespace CommitHelper.Domain.GItDiff.Service;

public class GitDiffService(IGitDiffRepository repository)
{
    public async Task<GitDiff.GitDiff> GetStagedDiffAsync(CancellationToken ct = default)
    {
        return await repository.GetAsync(ct);
    }
}
