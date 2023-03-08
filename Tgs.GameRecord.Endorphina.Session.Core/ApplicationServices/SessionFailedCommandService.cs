using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class SessionFailedCommandService : ICommandService<SessionFailedCommand>
{
    private readonly ISessionRepository _repository;

    public SessionFailedCommandService(ISessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ValueTask ExecuteAsync(SessionFailedCommand command, CancellationToken cancellationToken = default)
        => _repository.UpdateFailedAsync(command.Token, cancellationToken);
}
