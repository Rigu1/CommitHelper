namespace CommitHelper.Infra.Adapters;

public interface IAiAdapter
{
    Task<string> GenerateContentAsync(string prompt, CancellationToken ct = default);
}
