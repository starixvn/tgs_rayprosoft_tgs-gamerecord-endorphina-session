namespace Tgs.GameRecord.Endorphina.Session.Core.Models;
public record Session(
    string Token,
    TokenState State,
    string BrandCode,
    string PlayerCode,
    int Retry,
    string MerchantId);
