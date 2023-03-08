using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class SessionGetService : IGetService<SessionGet, SessionInfo?>
{
    private readonly ISessionRepository _repository;

    public SessionGetService(ISessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async ValueTask<SessionInfo?> GetAsync(SessionGet query, CancellationToken cancellationToken = default)
    {
        var result = await _repository
            .GetAsync(query.Token, cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            return null;

        return new SessionInfo(result!.Token, result.State, result.BrandCode, result.PlayerCode, result.Retry);
    }
}
