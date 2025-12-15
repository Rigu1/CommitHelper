namespace CommitHelper.Infra.Git.Constants;

public static class GitErrors
{
    // 정적 메서드로 메시지 생성
    public static string CommandFailed(string errorOutput)
    {
        return $"Git 명령 실행 실패: {errorOutput}";
    }
}
