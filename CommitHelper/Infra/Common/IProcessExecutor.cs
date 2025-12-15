using System.Diagnostics;
using CommitHelper.Infra.Common.Dto;

namespace CommitHelper.Infra.Common;

public interface IProcessExecutor
{
    Task<ProcessResult> RunAsync(ProcessStartInfo info, CancellationToken ct);
}
