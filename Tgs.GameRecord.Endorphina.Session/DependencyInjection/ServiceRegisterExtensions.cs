using Tgs.GameRecord.Endorphina.Session.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegisterExtensions
{
    public static async Task<WebApplication> UseRegister(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var scheduleFormRepository = scope.ServiceProvider.GetRequiredService<ISessionRepository>();

            await scheduleFormRepository
                .InitDBAsync()
                .ConfigureAwait(false);
        }
        return app;
    }
}
