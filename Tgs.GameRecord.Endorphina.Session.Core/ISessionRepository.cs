namespace Tgs.GameRecord.Endorphina.Session.Core;

public interface ISessionRepository
{
	ValueTask InitDBAsync(CancellationToken cancellationToken = default);

	ValueTask AddAsync(
		string token,
		string brandCode,
		string playerCode,
		string merchantId,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<Models.Session> QueryAsync(
		TokenState state,
		string merchantId,
		CancellationToken cancellationToken = default);

	ValueTask<Models.Session?> GetAsync(
		string token,
		CancellationToken cancellationToken = default);

	ValueTask UpdateRetryAsync(
		string token,
		CancellationToken cancellationToken = default);

	ValueTask UpdateCompleteAsync(
		string token,
		CancellationToken cancellationToken = default);

	ValueTask UpdateFailedAsync(
		string token,
		CancellationToken cancellationToken = default);

	ValueTask UpdateProcessingAsync(
		string token,
		CancellationToken cancellationToken = default);

	ValueTask DeleteAsync(
		string token,
		CancellationToken cancellationToken = default);
}
