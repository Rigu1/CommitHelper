using CommitHelper.Domain.GitDiff;
using CommitHelper.Domain.GItDiff.Service;
using Moq;

namespace CommitHelper.Tests.Domain.GitDiff.Service;

public class GitDiffServiceTests
{
    private readonly Mock<IGitDiffRepository> _mockRepository;
    private readonly GitDiffService _service;

    public GitDiffServiceTests()
    {
        _mockRepository = new Mock<IGitDiffRepository>();
        _service = new GitDiffService(_mockRepository.Object);
    }

    [Fact(DisplayName = "리포지토리에서 반환한 GitDiff 객체를 그대로 반환해야 한다.")]
    public async Task GetStagedDiff_ShouldReturnDiff_FromRepository()
    {
        var validContent = "diff --git a/Program.cs b/Program.cs ...";

        var expectedDiff = new CommitHelper.Domain.GitDiff.GitDiff(validContent);

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDiff);

        var result = await _service.GetStagedDiffAsync();

        Assert.NotNull(result);
        Assert.Equal(expectedDiff.Content, result.Content);

        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "리포지토리에서 예외(Git 미설치 등)가 발생하면 그대로 전파해야 한다.")]
    public async Task GetStagedDiff_ShouldPropagateException_WhenRepositoryFails()
    {
        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FileNotFoundException("Git이 설치되어 있지 않습니다."));

        await Assert.ThrowsAsync<FileNotFoundException>(() => _service.GetStagedDiffAsync());

        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
