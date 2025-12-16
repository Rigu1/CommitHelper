using CommitHelper.Domain.Exceptions;
using CommitHelper.Infra.Common;
using CommitHelper.Infra.Common.Dto;
using System.Diagnostics;
using CommitHelper.Domain.Commit.repository;

namespace CommitHelper.Infra.Repositories.Git;

public class GitCommitRepository(IProcessExecutor processExecutor) : IGitCommitRepository
{
    public async Task CommitAsync(string message)
    {
        var escapedMessage = message.Replace("\"", "\\\"");
        var arguments = $"commit -m \"{escapedMessage}\"";

        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments
        };

        ProcessResult result;
        try
        {
            result = await processExecutor.RunAsync(startInfo, CancellationToken.None);
        }
        catch (Exception ex)
        {
            throw new GitCommitException($"Git commit 명령 실행 중 인프라 오류: {ex.Message}");
        }

        if (result.ExitCode != 0)
        {
            throw new GitCommitException(
                $"Git commit 실패 (Exit Code: {result.ExitCode})." +
                $"\nError Details: {result.Error}"
            );
        }
    }
}
