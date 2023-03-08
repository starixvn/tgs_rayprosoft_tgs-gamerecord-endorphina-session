using System.Runtime.CompilerServices;
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

    public async ValueTask MoveFailedAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new MoveFailedRequest()
        {
            Token = token,
        };

        _ = await _client
            .MoveFailedAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<Protos.Session> QuerySessionsAsync(TokenState state, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new QueryRequest()
        {
            State = state,
        };

        var reply = await _client
            .QueryAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async ValueTask RemoveSessionAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new RemoveRequest()
        {
            Token = token,
        };

        _ = await _client
            .RemoveAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask UpdateSessionCompleteAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new CompleteRequest()
        {
            Token = token,
        };

        _ = await _client
            .CompleteAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask UpdateSessionFailedAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new FailedRequest()
        {
            Token = token,
        };

        _ = await _client
            .FailedAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask UpdateSessionProcessingAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new ProcessingRequest()
        {
            Token = token,
        };

        _ = await _client
            .ProcessingAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask UpdateSessionRetryAsync(string token, CancellationToken cancellationToken = default)
    {
        var request = new UpdateRetryRequest()
        {
            Token = token,
        };

        _ = await _client.UpdateRetryAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
