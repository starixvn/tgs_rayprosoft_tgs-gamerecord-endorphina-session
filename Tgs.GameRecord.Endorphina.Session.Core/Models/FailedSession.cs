namespace Tgs.GameRecord.Endorphina.Session.Core.Models;
public record FailedSession(
    string Token,
    string BrandCode,
    string PlayerCode,
    string MerchantId);
