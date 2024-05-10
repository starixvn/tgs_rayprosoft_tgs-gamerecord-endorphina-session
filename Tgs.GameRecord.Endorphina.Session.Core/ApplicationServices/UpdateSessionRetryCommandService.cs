using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class UpdateSessionRetryCommandService : ICommandService<UpdateSessionRetryCommand>
{
	private readonly ISessionRepository _repository;

	public UpdateSessionRetryCommandService(ISessionRepository repository)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}

	public ValueTask ExecuteAsync(UpdateSessionRetryCommand command, CancellationToken cancellationToken = default)
		=> _repository.UpdateRetryAsync(command.Token, cancellationToken);
}
