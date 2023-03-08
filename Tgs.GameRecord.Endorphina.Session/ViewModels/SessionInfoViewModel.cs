using Tgs.GameRecord.Endorphina.Session.Common;

namespace Tgs.GameRecord.Endorphina.Session.ViewModels;

public record SessionInfoViewModel(
    string Token,
    TokenState State,
    string BrandCode,
    string PlayerCode,
    int Retry);
