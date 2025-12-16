using CommitHelper.Domain.Exceptions;
using CommitHelper.Infra.Common;
using CommitHelper.Infra.Common.Dto;
using System.Diagnostics;
using CommitHelper.Domain.Commit.Repository;
using CommitHelper.Infra.Repositories.Git.Constants;

namespace CommitHelper.Infra.Repositories.Git;

public class GitCommitRepository(IProcessExecutor processExecutor) : IGitCommitRepository
{
    public async Task CommitAsync(string message)
    {
        var arguments = CreateCommitArguments(message);

        var startInfo = new ProcessStartInfo
        {
            FileName = GitConstants.Command,
            Arguments = arguments
        };

        var result = await ExecuteProcessAndHandleInfraError(startInfo);

        EnsureCommitSuccess(result);
    }

    private static string CreateCommitArguments(string message)
    {
        var escapedMessage = message.Replace("\"", "\\\"");
        return $"{GitConstants.ArgsCommit} \"{escapedMessage}\"";
    }

    private async Task<ProcessResult> ExecuteProcessAndHandleInfraError(ProcessStartInfo startInfo)
    {
        try
        {
            return await processExecutor.RunAsync(startInfo, CancellationToken.None);
        }
        catch (Exception ex)
        {
            throw new GitCommitException($"{GitConstants.InfraExecutionError}: {ex.Message}");
        }
    }

    private static void EnsureCommitSuccess(ProcessResult result)
    {
        if (result.ExitCode != 0)
        {
            throw new GitCommitException(
                GitConstants.CommitFailed(result.ExitCode, result.Error)
            );
        }
    }
}
