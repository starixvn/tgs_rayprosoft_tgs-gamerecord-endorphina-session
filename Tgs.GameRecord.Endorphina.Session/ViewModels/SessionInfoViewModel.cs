using Tgs.GameRecord.Endorphina.Session.Core;

namespace Tgs.GameRecord.Endorphina.Session.ViewModels;

public record SessionInfoViewModel(
    string Token,
    TokenState State,
    string BrandCode,
    string PlayerCode,
    int Retry,
    string MerchantId);
