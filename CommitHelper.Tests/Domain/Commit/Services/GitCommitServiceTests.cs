using CommitHelper.Domain.Commit;
using CommitHelper.Domain.Commit.Repository;
using CommitHelper.Domain.Commit.Services;
using Moq;

namespace CommitHelper.Tests.Domain.Commit.Services;

public class GitCommitServiceTests
{
    private readonly GitCommitService _service;
    private readonly Mock<IGitCommitRepository> _repositoryMock;

    public GitCommitServiceTests()
    {
        _repositoryMock = new Mock<IGitCommitRepository>();
        _service = new GitCommitService(_repositoryMock.Object);
    }

    [Fact(DisplayName = "확정된 CommitMessage가 주어지면, 해당 내용으로 커밋 실행을 요청해야 한다.")]
    public async Task CommitAsync_ShouldRequestCommitExecutionWithConfirmedMessageContent()
    {
        var confirmedMessageContent = "feat: 사용자 확정 메시지로 커밋 요청 테스트";
        var commitMessage = new CommitMessage(confirmedMessageContent);

        await _service.CommitAsync(commitMessage);

        _repositoryMock.Verify(
            repository => repository.CommitAsync(confirmedMessageContent),
            Times.Once,
            "서비스는 VO의 내용을 추출하여 커밋 리포지토리에 전달해야 합니다."
        );
    }
}
