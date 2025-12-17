using CommitHelper.Configuration;
using CommitHelper.Domain.Exceptions;
using CommitHelper.Domain.MessageGeneration;
using CommitHelper.Domain.MessageGeneration.Repository;
using CommitHelper.Domain.MessageGeneration.Services;
using Moq;

namespace CommitHelper.Tests.Domain.MessageGeneration.Services;

public class AICommitMessageServiceTests
{
    private readonly Mock<IAICommitMessageRepository> _aiRepositoryMock;
    private readonly AICommitMessageService _service;

    private const string MockFormattedDiff =
        "--- a/User.cs\n+++ b/User.cs\n@@ -1,1 +1,1 @@\n-public class User {}\n+public record User {};";

    private const string TestSystemPrompt = "당신은 한국어 커밋 전문가이며, 50자 이내의 간결한 제목을 제공합니다.";
    private const string MockGeneratedMessage = "refactor: 사용자 클래스를 레코드로 변경\n\n- 불변성을 확보하고 코드를 간결화했습니다.";

    public AICommitMessageServiceTests()
    {
        _aiRepositoryMock = new Mock<IAICommitMessageRepository>(MockBehavior.Strict);

        var testAiSettings = new AiSettings
        {
            ApiKey = "test_key",
            ModelType = "test-model",
            SystemPrompt = TestSystemPrompt
        };

        _service = new AICommitMessageService(
            _aiRepositoryMock.Object,
            testAiSettings
        );
    }

    private void SetupSuccessfulFlow(CancellationToken ct)
    {
        _aiRepositoryMock
            .Setup(r => r.GenerateMessageAsync(
                It.IsAny<string>(),
                ct))
            .ReturnsAsync(MockGeneratedMessage);
    }

    [Fact(DisplayName = "1. 성공 시, AI Repository를 호출하고 GeneratedMessage VO를 반환해야 한다.")]
    public async Task GenerateMessageAsync_WhenSuccessful_ShouldReturnGeneratedMessage()
    {
        var ct = CancellationToken.None;
        SetupSuccessfulFlow(ct);

        var result = await _service.GenerateMessageAsync(MockFormattedDiff, ct);

        Assert.NotNull(result);
        Assert.IsType<GeneratedMessage>(result);
        Assert.Equal(MockGeneratedMessage, result.Value);

        _aiRepositoryMock.Verify(r => r.GenerateMessageAsync(It.IsAny<string>(), ct), Times.Once);
    }

    [Fact(DisplayName = "2. AI Repository에 전달되는 프롬프트는 SystemPrompt와 Diff를 정확히 조합해야 한다.")]
    public async Task GenerateMessageAsync_ShouldCombineSystemPromptAndDiffContent()
    {
        var ct = CancellationToken.None;
        SetupSuccessfulFlow(ct);

        await _service.GenerateMessageAsync(MockFormattedDiff, ct);

        _aiRepositoryMock.Verify(r => r.GenerateMessageAsync(
                It.Is<string>(prompt =>
                    prompt.Contains($"[SYSTEM INSTRUCTION]:\n{TestSystemPrompt}") &&
                    prompt.Contains($"[GIT DIFF CONTENT]:\n{MockFormattedDiff}")),
                ct),
            Times.Once,
            "프롬프트는 설정된 지침과 Diff 내용을 명확한 포맷으로 결합해야 합니다.");
    }

    [Theory(DisplayName = "3. Diff 내용이 없거나 공백이면 InvalidOperationException을 던져야 한다.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GenerateMessageAsync_WhenDiffIsEmpty_ShouldThrowException(string emptyDiff)
    {
        var ct = CancellationToken.None;

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GenerateMessageAsync(emptyDiff, ct));

        _aiRepositoryMock.Verify(r => r.GenerateMessageAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "4. AI Repository에서 인증 예외 발생 시, 예외를 그대로 전파해야 한다.")]
    public async Task GenerateMessageAsync_WhenAiAuthFails_ShouldPropagateException()
    {
        var expectedException = new AiAuthenticationException("API 인증 정보가 유효하지 않습니다.");

        _aiRepositoryMock
            .Setup(r => r.GenerateMessageAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var thrownException = await Assert.ThrowsAsync<AiAuthenticationException>(() =>
            _service.GenerateMessageAsync(MockFormattedDiff));

        Assert.Equal(expectedException, thrownException);
        _aiRepositoryMock.Verify(r => r.GenerateMessageAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
