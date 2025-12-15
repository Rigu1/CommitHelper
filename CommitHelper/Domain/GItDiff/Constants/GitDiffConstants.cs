namespace CommitHelper.Domain.GitDiff.Constants;

public static class GitDiffConstants
{
    public const int MaxContentLength = 1000;
    public const string ErrorEmptyContent = "스테이징된 변경 사항이 없습니다.";
    public const string ErrorTooLongContentFormat = "Diff 내용이 너무 깁니다. (길이: {0}, 제한: {1})";
}
