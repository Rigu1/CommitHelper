using System.Diagnostics;
using CommitHelper.Infra.Common.Config;
using CommitHelper.Infra.Common.Dto;

namespace CommitHelper.Infra.Common;

public class ProcessExecutor
{
    public async Task<ProcessResult> RunAsync(ProcessStartInfo info, CancellationToken ct)
    {
        info.ApplyBaseSettings();

        using var process = new Process();
        process.StartInfo = info;

        if (!process.Start()) throw new InvalidOperationException("프로세스 시작 실패");

        return await CaptureOutputAsync(process, ct);
    }

    private async Task<ProcessResult> CaptureOutputAsync(Process process, CancellationToken ct)
    {
        var stdoutTask = process.StandardOutput.ReadToEndAsync(ct);
        var stderrTask = process.StandardError.ReadToEndAsync(ct);

        await process.WaitForExitAsync(ct);

        return new ProcessResult(
            process.ExitCode,
            (await stdoutTask).Trim(),
            (await stderrTask).Trim()
        );
    }
}
