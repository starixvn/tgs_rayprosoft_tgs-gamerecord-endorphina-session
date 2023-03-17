using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class SessionQueryService : IQueryService<SessionQuery, SessionInfo>
{
    private readonly ISessionRepository _repository;

    public SessionQueryService(ISessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<SessionInfo> QueryAsync(SessionQuery query, CancellationToken cancellationToken = default)
        => _repository
            .QueryAsync(query.State, query.MerchantId, cancellationToken)
            .Select(x => new SessionInfo(x.Token, x.State, x.BrandCode, x.PlayerCode, x.Retry, x.MerchantId));
}
