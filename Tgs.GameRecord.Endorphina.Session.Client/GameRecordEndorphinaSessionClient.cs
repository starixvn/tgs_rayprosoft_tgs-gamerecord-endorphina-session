using Tgs.GameRecord.Endorphina.Session.Protos;

namespace Tgs.GameRecord.Endorphina.Session.Client;

internal class GameRecordEndorphinaSessionClient : IGameRecordEndorphinaSessionClient
{
    private readonly GameRecordEndorphinaSession.GameRecordEndorphinaSessionClient _client;

    public GameRecordEndorphinaSessionClient(
        GameRecordEndorphinaSession.GameRecordEndorphinaSessionClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask AddSessionAsync(string token, string brandCode, string playerCode, CancellationToken cancellationToken = default)
    {
        var request = new AddRequest
        {
            Token = token,
            BrandCode = brandCode,
            PlayerCode = playerCode
        };

        _ = await _client.AddAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<Protos.Session> GetSessionAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new GetRequest()
        {
            Token = token,
        };

        var session = await _client
            .GetAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return session;
    }

    public ValueTask MoveFailedAsync(string token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Protos.Session> QuerySessionsAsync(Common.TokenState state, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask RemoveSessionAsync(string token, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask UpdateSessionCompleteAsync(string token, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask UpdateSessionFailedAsync(string token, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask UpdateSessionProcessingAsync(string token, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask UpdateSessionRetryAsync(string token, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
