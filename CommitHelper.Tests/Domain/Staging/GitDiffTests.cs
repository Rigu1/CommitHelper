using CommitHelper.Domain.Staging;
using CommitHelper.Domain.Staging.Constants;

namespace CommitHelper.Tests.Domain.Staging;

public class GitDiffTests
{
    [Theory(DisplayName = "변경 사항이 없거나(Null) 공백이면 ArgumentException이 발생한다.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidContent_ShouldThrowException(string invalidContent)
    {
        Assert.Throws<ArgumentException>(() => new GitDiff(invalidContent));
    }

    [Fact(DisplayName = "내용이 길이 제한을 초과하면 ArgumentException이 발생한다.")]
    public void Constructor_WithTooLongContent_ShouldThrowException()
    {
        var longContent = new string('a', GitDiffConstants.MaxContentLength + 1);

        Assert.Throws<ArgumentException>(() => new GitDiff(longContent));
    }

    [Fact(DisplayName = "유효한 Git Diff 문자열로 객체가 정상적으로 생성된다.")]
    public void Constructor_WithValidContent_ShouldSucceed()
    {
        var validContent = "diff --git a/Program.cs ...";

        var gitDiff = new GitDiff(validContent);

        Assert.NotNull(gitDiff);
        Assert.Equal(validContent, gitDiff.Content);
    }
}
