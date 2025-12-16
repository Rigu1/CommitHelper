using CommitHelper.Domain.Commit.Constants;

namespace CommitHelper.Domain.Commit;

public sealed class CommitMessage
{
    public string Value { get; }

    public CommitMessage(string value)
    {
        Validate(value);
        Value = value;
    }

    private static void Validate(string value)
    {
        EnsureValueIsNotEmpty(value);
    }

    private static void EnsureValueIsNotEmpty(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(CommitMessageConstants.ErrorEmptyValue, nameof(value));
        }
    }
}
