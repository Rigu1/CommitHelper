using CommitHelper.Configuration;
using CommitHelper.Domain.Commit.Repository;
using CommitHelper.Domain.Commit.Services;
using CommitHelper.Domain.MessageGeneration.Repository;
using CommitHelper.Domain.MessageGeneration.Services;
using CommitHelper.Domain.Staging.Repository;
using CommitHelper.Domain.Staging.Services;
using CommitHelper.Domain.Staging.Converter;
using CommitHelper.Infra.Common;
using CommitHelper.Infra.Repositories.Git;
using CommitHelper.Presentation.View;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using CommitHelper.Infra.Adapters;
using CommitHelper.Infra.Repositories.AI;
using CommitHelper.Presentation;

namespace CommitHelper;

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        var configuration = ConfigureConfiguration();

        ConfigureServices(services, configuration);

        using var serviceProvider = services.BuildServiceProvider();

        var controller = serviceProvider.GetRequiredService<CommitController>();
        await controller.RunAsync();
    }

    private static IConfiguration ConfigureConfiguration()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        return new ConfigurationBuilder()
            .SetBasePath(projectRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<AiSettings>(configuration.GetSection("AiSettings"));
        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<AiSettings>>().Value;
            settings.EnsureValid();
            return settings;
        });

        AddInfraServices(services);
        AddDomainServices(services);
        AddPresentationServices(services);
    }

    private static void AddInfraServices(IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddSingleton<IProcessExecutor, ProcessExecutor>();
        services.AddSingleton<IAiAdapter, GeminiAdapter>();
        services.AddSingleton<IAICommitMessageRepository, AiCommitMessageRepository>();
        services.AddSingleton<IGitCommitRepository, GitCommitRepository>();
        services.AddSingleton<IGitDiffRepository, GitDiffRepository>();
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        services.AddSingleton<GitDiffConverter>();
        services.AddSingleton<GitDiffService>();
        services.AddSingleton<AICommitMessageService>();
        services.AddSingleton<GitCommitService>();
    }

    private static void AddPresentationServices(IServiceCollection services)
    {
        services.AddSingleton<InputView>();
        services.AddSingleton<OutputView>();
        services.AddSingleton<CommitController>();
    }
}
