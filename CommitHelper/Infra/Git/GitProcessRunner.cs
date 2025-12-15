using System.ComponentModel;
using System.Diagnostics;
using CommitHelper.Domain.GitDiff;
using CommitHelper.Infra.Common;
using CommitHelper.Infra.Common.Dto;

namespace CommitHelper.Infra.Git;

public class GitDiffRepository(string workingDirectory = ".") : IGitDiffRepository
{
    private readonly ProcessExecutor _executor = new();
    private const string GitPath = "git";

    public async Task<GitDiff> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var info = CreateStartInfo();
            var result = await _executor.RunAsync(info, ct);

            ValidateResult(result);

            return new GitDiff(result.Output);
        }
        catch (Win32Exception)
        {
            throw new FileNotFoundException("Git이 설치되어 있지 않습니다.");
        }
    }

    private ProcessStartInfo CreateStartInfo()
    {
        return new ProcessStartInfo
        {
            FileName = GitPath,
            Arguments = "diff --staged",
            WorkingDirectory = workingDirectory
        };
    }

    private void ValidateResult(ProcessResult result)
    {
        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException($"Git 명령 실패: {result.Error}");
        }
    }
}
