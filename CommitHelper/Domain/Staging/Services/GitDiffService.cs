using CommitHelper.Domain.Staging.Converter;
using CommitHelper.Domain.Staging.Repository;

namespace CommitHelper.Domain.Staging.Services;

public class GitDiffService(IGitDiffRepository repository, GitDiffConverter converter)
{
    /*
    public async Task<GitDiff> GetStagedDiffAsync(CancellationToken ct = default)
    {
        return await repository.GetAsync(ct);
    }
    */

    public virtual async Task<string> GetDiffAsAiPromptAsync(CancellationToken ct = default)
    {
        var diff = await repository.GetAsync(ct);

        return converter.ConvertToAiFormat(diff);
    }
}
