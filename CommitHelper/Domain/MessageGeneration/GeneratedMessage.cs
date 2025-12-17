using CommitHelper.Domain.Commit.Constants;
using CommitHelper.Domain.MessageGeneration.Constants;

namespace CommitHelper.Domain.MessageGeneration;

public sealed record GeneratedMessage
{
    public string Value { get; }

    public GeneratedMessage(string rawText)
    {
        Validate(rawText);
        Value = rawText.Trim();
    }

    private static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(MessageGenerationConstants.ErrorEmptyAiResponse, nameof(value));
        }
    }
}
