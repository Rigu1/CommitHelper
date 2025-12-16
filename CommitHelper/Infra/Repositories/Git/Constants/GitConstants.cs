namespace CommitHelper.Infra.Git.Constants;

public static class GitConstants
{
    public const string Command = "git";
    public const string ArgsDiffStaged = "diff --staged";

    public static string CommandFailed(string errorOutput)
    {
        return $"Git 명령 실행 실패: {errorOutput}";
    }
}
