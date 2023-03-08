using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class SessionCompleteCommandService : ICommandService<SessionCompleteCommand>
{
    private readonly ISessionRepository _repository;

    public SessionCompleteCommandService(ISessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ValueTask ExecuteAsync(SessionCompleteCommand command, CancellationToken cancellationToken = default)
        => _repository.UpdateCompleteAsync(command.Token, cancellationToken);
}
