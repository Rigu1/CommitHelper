namespace CommitHelper.Domain.Staging.Constants;

public static class GitDiffErrors
{
    public static string TooLongContent(int currentLength, int limit)
    {
        return $"Diff 내용이 너무 깁니다. (길이: {currentLength}, 제한: {limit})";
    }
}
