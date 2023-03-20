namespace Tgs.GameRecord.Endorphina.Session.Core;

public interface IFailedSessionRepository
{
    ValueTask InitDBAsync(CancellationToken cancellationToken = default);

    ValueTask AddFailedAsync(
        string token,
        string brandCode,
        string playerCode,
        string merchantId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<Models.FailedSession> QueryFailedAsync(
        string merchantId,
        CancellationToken cancellationToken = default);
}
