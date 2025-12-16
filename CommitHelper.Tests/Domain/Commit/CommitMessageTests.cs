using CommitHelper.Domain.Commit;

namespace CommitHelper.Tests.Domain.Commit;

public class CommitMessageTests
{
    [Theory(DisplayName = "커밋 메시지가 없거나(Null) 공백이면 ArgumentException이 발생한다.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidValue_ShouldThrowException(string invalidValue)
    {
        Assert.Throws<ArgumentException>(() => new CommitMessage(invalidValue));
    }

    [Fact(DisplayName = "커밋 메시지로 객체가 생성된다.")]
    public void Constructor_WithValidValue_ShouldSucceed()
    {
        var validValue = "feat: 로그 기능 추가";

        var commitMessage = new CommitMessage(validValue);

        Assert.NotNull(commitMessage);
        Assert.Equal(validValue, commitMessage.Value);
    }
}
