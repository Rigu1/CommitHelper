using System.ComponentModel;
using System.Diagnostics;
using CommitHelper.Domain.Exceptions;
using CommitHelper.Domain.GitDiff;
using CommitHelper.Infra.Common;
using CommitHelper.Infra.Common.Dto;
using CommitHelper.Infra.Git.Constants;

namespace CommitHelper.Infra.Repositories.Git;

public class GitDiffRepository(IProcessExecutor executor, string workingDirectory = ".") : IGitDiffRepository
{
    public async Task<GitDiff> GetAsync(CancellationToken ct = default)
    {
        var rawOutput = await CaptureDiffOutputAsync(ct);

        return new GitDiff(rawOutput);
    }

    private async Task<string> CaptureDiffOutputAsync(CancellationToken ct)
    {
        try
        {
            var info = CreateStartInfo();
            var result = await executor.RunAsync(info, ct);

            ValidateResult(result);

            return result.Output;
        }
        catch (Win32Exception)
        {
            throw new GitNotFoundException();
        }
    }

    private ProcessStartInfo CreateStartInfo()
    {
        return new ProcessStartInfo
        {
            FileName = GitConstants.Command,
            Arguments = GitConstants.ArgsDiffStaged,
            WorkingDirectory = workingDirectory
        };
    }

    private static void ValidateResult(ProcessResult result)
    {
        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException(GitErrors.CommandFailed(result.Error));
        }
    }
}
