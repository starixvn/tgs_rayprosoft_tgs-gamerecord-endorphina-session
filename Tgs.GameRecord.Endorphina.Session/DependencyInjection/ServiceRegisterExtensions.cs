using Tgs.GameRecord.Endorphina.Session.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegisterExtensions
{
	public static async Task<WebApplication> UseRegister(this WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var sessionRepository = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
			var failedSessionRepository = scope.ServiceProvider.GetRequiredService<IFailedSessionRepository>();

			await sessionRepository
				.InitDBAsync()
				.ConfigureAwait(false);

			await failedSessionRepository
				.InitDBAsync()
				.ConfigureAwait(false);
		}

		return app;
	}
}
