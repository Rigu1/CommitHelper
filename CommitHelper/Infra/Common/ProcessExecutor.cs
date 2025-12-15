using System.Diagnostics;
using CommitHelper.Infra.Common.Config;
using CommitHelper.Infra.Common.Constants;
using CommitHelper.Infra.Common.Dto;

namespace CommitHelper.Infra.Common;

public class ProcessExecutor : IProcessExecutor
{
    private static async Task<ProcessResult> CaptureOutputAsync(Process process, CancellationToken ct)
    {
        var stdoutTask = process.StandardOutput.ReadToEndAsync(ct);
        var stderrTask = process.StandardError.ReadToEndAsync(ct);

        await process.WaitForExitAsync(ct);

        return new ProcessResult(
            ExitCode: process.ExitCode,
            Output: (await stdoutTask).Trim(),
            Error: (await stderrTask).Trim()
        );
    }

    public async Task<ProcessResult> RunAsync(ProcessStartInfo info, CancellationToken ct)
    {
        info.ApplyBaseSettings();

        using var process = new Process();
        process.StartInfo = info;

        EnsureProcessStarted(process, info);

        return await CaptureOutputAsync(process, ct);
    }

    private static void EnsureProcessStarted(Process process, ProcessStartInfo info)
    {
        if (!process.Start())
        {
            throw new InvalidOperationException(ProcessConstants.StartFailure(
                info.FileName,
                info.Arguments
            ));
        }
    }
}
