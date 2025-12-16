using Microsoft.Extensions.DependencyInjection;
using CommitHelper.Domain.Staging;
using CommitHelper.Domain.Staging.Repository;
using CommitHelper.Domain.Staging.Services;
using CommitHelper.Infra.Common;
using CommitHelper.Infra.Repositories.Git;

namespace CommitHelper;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();

        await RunApplicationAsync(serviceProvider);
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IProcessExecutor, ProcessExecutor>();
        services.AddSingleton<IGitDiffRepository, GitDiffRepository>();
        services.AddSingleton<GitDiffService>();

        return services;
    }

    private static async Task RunApplicationAsync(ServiceProvider serviceProvider)
    {
        var diffService = serviceProvider.GetRequiredService<GitDiffService>();

        try
        {
            Console.Write("Staged 변경 사항 조회 중... ");

            var diff = await diffService.GetStagedDiffAsync();

            Console.WriteLine("완료\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== [Git Diff 결과] ===");
            Console.ResetColor();

            Console.WriteLine(diff.Content);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ERROR] {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {ex.Message}");
            Console.ResetColor();
        }
        finally
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("프로그램 종료.");
        }
    }
}
