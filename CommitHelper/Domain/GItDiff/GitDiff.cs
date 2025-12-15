namespace CommitHelper.Domain.GitDiff;

public sealed class GitDiff
{
    private const int MaxContentLength = 1000;
    public string Content { get; }

    public GitDiff(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("스테이징된 변경 사항이 없습니다.", nameof(content));

        if (content.Length > MaxContentLength)
            throw new ArgumentException($"변경 사항이 너무 많습니다. (길이: {content.Length})", nameof(content));

        Content = content;
    }
}
