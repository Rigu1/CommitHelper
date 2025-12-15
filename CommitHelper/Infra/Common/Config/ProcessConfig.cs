using System.Diagnostics;
using System.Text;

namespace CommitHelper.Infra.Common.Config;

public static class ProcessConfigurator
{
    public static void ApplyBaseSettings(this ProcessStartInfo info)
    {
        info.StandardOutputEncoding = Encoding.UTF8;
        info.StandardErrorEncoding = Encoding.UTF8;
        info.RedirectStandardOutput = true;
        info.RedirectStandardError = true;
        info.UseShellExecute = false;
        info.CreateNoWindow = true;
    }
}
