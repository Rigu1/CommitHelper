using CommitHelper.Domain.Staging.Constants;

namespace CommitHelper.Domain.Staging;

public sealed class GitDiff
{
    public string Content { get; }

    public GitDiff(string content)
    {
        Validate(content);
        Content = content;
    }

    private static void Validate(string content)
    {
        EnsureContentIsNotEmpty(content);
        EnsureContentIsNotTooLong(content);
    }

    private static void EnsureContentIsNotEmpty(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException(GitDiffConstants.ErrorEmptyContent, nameof(content));
        }
    }

    private static void EnsureContentIsNotTooLong(string content)
    {
        if (content.Length <= GitDiffConstants.MaxContentLength) return;

        var errorMessage = GitDiffConstants.TooLongContent(content.Length, GitDiffConstants.MaxContentLength);

        throw new ArgumentException(errorMessage, nameof(content));
    }
}
