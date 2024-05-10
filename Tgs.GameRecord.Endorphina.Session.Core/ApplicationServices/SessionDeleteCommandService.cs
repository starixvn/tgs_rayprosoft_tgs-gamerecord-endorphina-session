using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class SessionDeleteCommandService : ICommandService<SessionDeleteCommand>
{
	private readonly ISessionRepository _repository;

	public SessionDeleteCommandService(ISessionRepository repository)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}

	public ValueTask ExecuteAsync(SessionDeleteCommand command, CancellationToken cancellationToken = default)
		=> _repository.DeleteAsync(command.Token, cancellationToken);
}
