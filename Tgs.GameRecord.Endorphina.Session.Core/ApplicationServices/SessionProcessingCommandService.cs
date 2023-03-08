using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class SessionProcessingCommandService : ICommandService<SessionProcessingCommand>
{
    private readonly ISessionRepository _repository;

    public SessionProcessingCommandService(ISessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ValueTask ExecuteAsync(SessionProcessingCommand command, CancellationToken cancellationToken = default)
        => _repository.UpdateProcessingAsync(command.Token, cancellationToken);
}
