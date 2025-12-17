namespace CommitHelper.Configuration;

public record AiSettings
{
    public required string ApiKey { get; init; }
    public required string ModelType { get; init; }
    public required string SystemPrompt { get; init; }

    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new InvalidOperationException("AiSettings: API Key는 비어 있을 수 없습니다.");

        if (string.IsNullOrWhiteSpace(SystemPrompt))
            throw new InvalidOperationException("AiSettings: System Prompt는 비어 있을 수 없습니다.");
    }
}
