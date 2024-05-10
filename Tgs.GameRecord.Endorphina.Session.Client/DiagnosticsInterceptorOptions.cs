using System.Diagnostics;

namespace Tgs.GameRecord.Endorphina.Session.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameRecord.Endorphina.Session.Client.DiagnosticsInterceptor");
}
