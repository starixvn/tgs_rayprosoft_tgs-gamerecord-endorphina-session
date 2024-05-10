using MongoDB.Driver;
using Tgs.GameRecord.Endorphina.Session.Core;
using Tgs.GameRecord.Endorphina.Session.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSessionRepositories(
		this IServiceCollection services,
		Func<IServiceProvider, IMongoDatabase> mongoDatabaseResolver)
		=> services
		.AddTransient<ISessionRepository>((sp) => ActivatorUtilities.CreateInstance<SessionRepository>(sp, mongoDatabaseResolver(sp)))
		.AddTransient<IFailedSessionRepository>((sp) => ActivatorUtilities.CreateInstance<FailedSessionRepository>(sp, mongoDatabaseResolver(sp)));
}
