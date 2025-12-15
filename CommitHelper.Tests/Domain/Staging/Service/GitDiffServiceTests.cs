using CommitHelper.Domain.Exceptions;
using CommitHelper.Domain.Staging;
using CommitHelper.Domain.Staging.Repository; // 네임스페이스 확인 필요
using CommitHelper.Domain.Staging.Service;    // 네임스페이스 확인 필요
using Moq;

namespace CommitHelper.Tests.Domain.Staging.Service;

public class GitDiffServiceTests
{
    public class GetStagedDiffAsync
    {
        private readonly Mock<IGitDiffRepository> _repositoryMock;
        private readonly GitDiffService _service;

        public GetStagedDiffAsync()
        {
            _repositoryMock = new Mock<IGitDiffRepository>();
            _service = new GitDiffService(_repositoryMock.Object);
        }

        [Fact(DisplayName = "리포지토리에서 GitNotFoundException이 발생하면, 예외를 그대로 리턴한다.")]
        public async Task WhenGitIsMissing_ShouldPropagateException()
        {
            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new GitNotFoundException());

            await Assert.ThrowsAsync<GitNotFoundException>(() => _service.GetStagedDiffAsync());
        }

        [Fact(DisplayName = "리포지토리가 성공적으로 데이터를 반환하면, 해당 Diff 객체를 반환한다.")]
        public async Task WhenRepositorySucceeds_ShouldReturnGitDiff()
        {
            var stagedContent = "diff --git a/Program.cs ...";
            var expectedDiff = new GitDiff(stagedContent);

            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDiff);

            var result = await _service.GetStagedDiffAsync();

            Assert.NotNull(result);
            Assert.Same(expectedDiff, result);
        }
    }
}
