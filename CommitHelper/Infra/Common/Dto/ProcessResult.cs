namespace CommitHelper.Infra.Common.Dto;

public record ProcessResult(int ExitCode, string Output, string Error);
