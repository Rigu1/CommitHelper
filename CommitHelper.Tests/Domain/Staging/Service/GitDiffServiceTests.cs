using CommitHelper.Domain.Exceptions;
using CommitHelper.Domain.Staging;
using CommitHelper.Domain.Staging.Converter;
using CommitHelper.Domain.Staging.Repository;
using CommitHelper.Domain.Staging.Services;
using Moq;
using Xunit;

namespace CommitHelper.Tests.Domain.Staging.Service;

public class GitDiffServiceTests
{
    public class GetDiffAsAiPromptAsync
    {
        private readonly Mock<IGitDiffRepository> _repositoryMock;
        private readonly Mock<GitDiffConverter> _converterMock;
        private readonly GitDiffService _service;

        public GetDiffAsAiPromptAsync()
        {
            _repositoryMock = new Mock<IGitDiffRepository>();
            _converterMock = new Mock<GitDiffConverter>();

            _service = new GitDiffService(_repositoryMock.Object, _converterMock.Object);
        }

        [Fact(DisplayName = "리포지토리에서 GitNotFoundException이 발생하면, 예외를 그대로 전파한다.")]
        public async Task WhenGitIsMissing_ShouldPropagateException()
        {
            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new GitNotFoundException());

            await Assert.ThrowsAsync<GitNotFoundException>(() => _service.GetDiffAsAiPromptAsync());

            _converterMock.Verify(conv => conv.ConvertToAiFormat(It.IsAny<GitDiff>()), Times.Never);
        }

        [Fact(DisplayName = "리포지토리가 성공하면, Converter를 호출하고 그 결과를 반환한다.")]
        public async Task WhenRepositorySucceeds_ShouldInvokeConverterAndReturnResult()
        {
            var rawContent = "diff --git a/Program.cs ...";
            var rawDiff = new GitDiff(rawContent);
            var convertedString =
                "--- GIT DIFF START ---\ndiff --git a/Program.cs ...\n--- GIT DIFF END ---";

            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rawDiff);

            _converterMock.Setup(conv => conv.ConvertToAiFormat(rawDiff))
                .Returns(convertedString);

            var result = await _service.GetDiffAsAiPromptAsync();

            _converterMock.Verify(conv => conv.ConvertToAiFormat(rawDiff), Times.Once,
                "서비스는 Repository의 결과를 Converter에게 전달해야 합니다.");

            Assert.Equal(convertedString, result);
        }

        [Fact(DisplayName = "리포지토리가 빈 문자열을 반환하면, GitDiff 생성자 예외를 전파한다.")]
        public async Task WhenRepositoryReturnsEmptyString_ShouldPropagateArgumentExceptionFromDomain()
        {
            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(
                    "Content cannot be empty or white space.",
                    "content"));

            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetDiffAsAiPromptAsync());

            _converterMock.Verify(conv => conv.ConvertToAiFormat(It.IsAny<GitDiff>()), Times.Never);
        }
    }
}
