namespace CommitHelper.Domain.Staging.Constants;

public static class GitDiffConstants
{
    public const int MaxContentLength = 50000;
    public const string ErrorEmptyContent = "스테이징된 변경 사항이 없습니다.";

    public static string TooLongContent(int currentLength, int limit)
    {
        return $"Diff 내용이 너무 깁니다. (길이: {currentLength}, 제한: {limit})";
    }
}
