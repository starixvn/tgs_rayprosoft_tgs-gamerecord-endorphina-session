namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;
public record SessionInfo(
	string Token,
	TokenState State,
	string BrandCode,
	string PlayerCode,
	int Retry,
	string MerchantId);
