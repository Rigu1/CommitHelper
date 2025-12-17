using CommitHelper.Domain.MessageGeneration;

namespace CommitHelper.Tests.Domain.MessageGeneration;

public class GeneratedMessageTests
{
    [Fact(DisplayName = "유효한 텍스트로 VO가 성공적으로 생성된다.")]
    public void WithValidText_ShouldCreateInstance()
    {
        const string rawText = "feat: 사용자 인증 기능 추가";

        var message = new GeneratedMessage(rawText);

        Assert.NotNull(message);
        Assert.Equal(rawText, message.Value);
    }

    [Fact(DisplayName = "공백이 포함된 텍스트는 앞뒤 공백을 제거하고 VO가 생성된다.")]
    public void WithWhitespacePadding_ShouldTrimContent()
    {
        const string rawText = " fix: 버그 수정 ";
        const string expectedText = "fix: 버그 수정";

        var message = new GeneratedMessage(rawText);

        Assert.Equal(expectedText, message.Value);
    }

    [Theory(DisplayName = "빈 문자열이나 공백만 입력 시 ArgumentException을 던진다.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\n")]
    public void WithNullOrWhiteSpace_ShouldThrowArgumentException(string invalidText)
    {
        Assert.Throws<ArgumentException>(() => new GeneratedMessage(invalidText));
    }
}
