namespace CommitHelper.Infra.Repositories.Git.Constants;

public static class GitConstants
{
    public const string Command = "git";
    public const string ArgsDiffStaged = "diff --staged";
    public const string ArgsCommit = "commit -m";

    public const string InfraExecutionError = "Git commit 명령 실행 중 인프라 오류";

    public static string CommandFailed(string errorOutput)
    {
        return $"Git 명령 실행 실패: {errorOutput}";
    }

    public static string CommitFailed(int exitCode, string errorDetails)
    {
        return $"Git commit 실패 (Exit Code: {exitCode}).\nError Details: {errorDetails}";
    }
}
