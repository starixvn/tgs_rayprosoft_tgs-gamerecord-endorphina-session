using Tgs.GameRecord.Endorphina.Session.Protos;

namespace Tgs.GameRecord.Endorphina.Session.Client;

public interface IGameRecordEndorphinaSessionClient
{
    ValueTask AddSessionAsync(
        string token,
        string brandCode,
        string playerCode,
        string merchantId,
        CancellationToken cancellationToken = default);

    ValueTask<Protos.Session> GetSessionAsync(
        string token,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<Protos.Session> QuerySessionsAsync(
        TokenState state,
        string merchantId,
        CancellationToken cancellationToken = default);

    ValueTask UpdateSessionCompleteAsync(
        string token,
        CancellationToken cancellationToken = default);

    ValueTask UpdateSessionFailedAsync(
        string token,
        CancellationToken cancellationToken = default);

    ValueTask UpdateSessionProcessingAsync(
        string token,
        CancellationToken cancellationToken = default);

    ValueTask UpdateSessionRetryAsync(
        string token,
        CancellationToken cancellationToken = default);

    ValueTask RemoveSessionAsync(
        string token,
        CancellationToken cancellationToken = default);

    ValueTask MoveFailedAsync(
        string token,
        CancellationToken cancellationToken = default);
}
