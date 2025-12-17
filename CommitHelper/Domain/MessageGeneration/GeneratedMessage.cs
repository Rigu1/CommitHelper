namespace CommitHelper.Domain.MessageGeneration;

public sealed class GeneratedMessage
{
    public string Value { get; }

    public GeneratedMessage(string rawText)
    {
        if (string.IsNullOrWhiteSpace(rawText))
        {
            throw new ArgumentException("AI 응답 메시지는 비어 있거나 공백만 포함할 수 없습니다.", nameof(rawText));
        }

        Value = rawText.Trim();
    }
}
