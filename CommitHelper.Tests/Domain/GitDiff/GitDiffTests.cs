namespace CommitHelper.Tests.Domain.GitDiff;

public class GitDiffTests
{
    [Theory(DisplayName = "변경 사항이 없거나(Null) 공백이면 ArgumentException이 발생한다.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidContent_ShouldThrowException(string invalidContent)
    {
        Assert.Throws<ArgumentException>(() => new CommitHelper.Domain.GitDiff.GitDiff(invalidContent));
    }

    [Fact(DisplayName = "내용이 길이 제한(1000자)을 초과하면 ArgumentException이 발생한다.")]
    public void Constructor_WithTooLongContent_ShouldThrowException()
    {
        var longContent = new string('a', 1001);

        Assert.Throws<ArgumentException>(() => new CommitHelper.Domain.GitDiff.GitDiff(longContent));
    }

    [Fact(DisplayName = "유효한 Git Diff 문자열로 객체가 정상적으로 생성된다.")]
    public void Constructor_WithValidContent_ShouldSucceed()
    {
        var validContent = "diff --git a/Program.cs ...";

        var gitDiff = new CommitHelper.Domain.GitDiff.GitDiff(validContent);

        Assert.NotNull(gitDiff);
        Assert.Equal(validContent, gitDiff.Content);
    }
}

