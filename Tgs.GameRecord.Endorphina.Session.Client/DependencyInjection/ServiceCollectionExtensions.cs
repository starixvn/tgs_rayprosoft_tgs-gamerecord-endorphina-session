using Microsoft.Extensions.Options;
using Tgs.GameRecord.Endorphina.Session.Client;
using Tgs.GameRecord.Endorphina.Session.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecordEndorphinaSessionClient(
		this IServiceCollection services,
		Action<GameRecordEndorphinaSessionClientOptions, IServiceProvider> configure)
		=> services
			.AddOptions<GameRecordEndorphinaSessionClientOptions>()
			.Configure(configure)
			.Services
			.AddGrpcClient<GameRecordEndorphinaSession.GameRecordEndorphinaSessionClient>((sp, configureClient) =>
		   {
			   var options = sp.GetRequiredService<IOptions<GameRecordEndorphinaSessionClientOptions>>().Value;
			   configureClient.Address = options.BaseUrl;
		   })
			.ConfigureChannel((sp, configureChannel) =>
			{
				var handler = new SocketsHttpHandler
				{
					PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
					KeepAlivePingDelay = TimeSpan.FromSeconds(60),
					KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
					EnableMultipleHttp2Connections = true
				};
				configureChannel.HttpHandler = handler;
			})
			.AddInterceptor<LoggingInterceptor>()
			.Services
			.AddSingleton<DiagnosticsInterceptorOptions>()
			.AddSingleton<LoggingInterceptor>()
			.AddTransient<IGameRecordEndorphinaSessionClient, GameRecordEndorphinaSessionClient>();
}
