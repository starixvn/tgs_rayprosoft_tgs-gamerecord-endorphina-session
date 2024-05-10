using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class MoveFailedSessionCommandService : ICommandService<MoveFailedSessionCommand>
{
	private readonly ISessionRepository _repository;
	private readonly IFailedSessionRepository _failedSessionRepository;

	public MoveFailedSessionCommandService(
		ISessionRepository repository,
		IFailedSessionRepository failedSessionRepository)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		_failedSessionRepository = failedSessionRepository ?? throw new ArgumentNullException(nameof(failedSessionRepository));
	}

	public async ValueTask ExecuteAsync(MoveFailedSessionCommand command, CancellationToken cancellationToken = default)
	{
		var result = await _repository
			.GetAsync(command.Token, cancellationToken)
			.ConfigureAwait(false);

		if (result is null)
			return;

		await _failedSessionRepository
			.AddFailedAsync(result.Token, result.BrandCode, result.PlayerCode, result.MerchantId, cancellationToken)
			.ConfigureAwait(false);

		await _repository
			.DeleteAsync(command.Token, cancellationToken)
			.ConfigureAwait(false);
	}
}
