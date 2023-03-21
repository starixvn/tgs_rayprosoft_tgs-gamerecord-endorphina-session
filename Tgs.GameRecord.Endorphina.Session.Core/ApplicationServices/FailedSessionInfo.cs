namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;
public record FailedSessionInfo(
    string Token,
    string BrandCode,
    string PlayerCode,
    string MerchantId);
