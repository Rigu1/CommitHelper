namespace CommitHelper.Domain.Staging.Converter;

public class GitDiffConverter
{
    public virtual string ConvertToAiFormat(GitDiff diff)
    {
        var formattedString =
            $"""
             --- GIT DIFF START ---
             {diff.Content}
             --- GIT DIFF END ---
             """;

        return formattedString;
    }
}
