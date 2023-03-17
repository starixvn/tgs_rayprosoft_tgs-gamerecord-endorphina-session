namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;
public record AddSessionCommand(
    string Token,
    string BrandCode,
    string PlayerCode,
    string MerchantId);
