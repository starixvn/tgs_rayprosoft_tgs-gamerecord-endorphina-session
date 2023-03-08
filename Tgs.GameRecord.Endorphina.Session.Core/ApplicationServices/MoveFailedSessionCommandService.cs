using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class MoveFailedSessionCommandService : ICommandService<MoveFailedSessionCommand>
{
    private readonly ISessionRepository _repository;

    public MoveFailedSessionCommandService(ISessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async ValueTask ExecuteAsync(MoveFailedSessionCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _repository
            .GetAsync(command.Token, cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
            return;

        await _repository
            .AddFailedAsync(result.Token, result.BrandCode, result.PlayerCode, cancellationToken)
            .ConfigureAwait(false);

        await _repository
            .DeleteAsync(command.Token, cancellationToken)
            .ConfigureAwait(false);
    }
}
