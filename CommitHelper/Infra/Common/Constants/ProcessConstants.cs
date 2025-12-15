namespace CommitHelper.Infra.Common.Constants;

public static class ProcessConstants
{
    public static string StartFailure(string fileName, string arguments)
    {
        return $"프로세스를 시작할 수 없습니다. (명령: {fileName} {arguments})";
    }
}
