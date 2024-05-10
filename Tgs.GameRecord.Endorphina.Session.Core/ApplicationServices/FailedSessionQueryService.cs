using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class FailedSessionQueryService : IQueryService<FailedSessionQuery, FailedSessionInfo>
{
	private readonly IFailedSessionRepository _repository;

	public FailedSessionQueryService(IFailedSessionRepository repository)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}

	public IAsyncEnumerable<FailedSessionInfo> QueryAsync(FailedSessionQuery query, CancellationToken cancellationToken = default)
		=> _repository
			.QueryFailedAsync(query.MerchantId, cancellationToken)
			.Select(x => new FailedSessionInfo(x.Token, x.BrandCode, x.PlayerCode, x.MerchantId));
}
