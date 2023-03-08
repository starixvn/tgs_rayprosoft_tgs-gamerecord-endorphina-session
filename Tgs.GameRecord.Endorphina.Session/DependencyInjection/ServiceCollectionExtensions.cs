using Tgs.GameRecord.Endorphina.Session;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecordEndorphinaSession(
        this IServiceCollection services,
        Action<MongoDBOptions, IServiceProvider> mongoDBconfigure)
        => services
            .AddOptions<MongoDBOptions>()
            .Configure(mongoDBconfigure)
            .Services
            .AddSingleton<MongoDBConnectionManager>()
            .AddGameRecordEndorphinaSessionCore()
            .AddSessionRepositories(
                (sp) => sp.GetRequiredService<MongoDBConnectionManager>().Database);
}
