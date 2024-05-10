using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

internal class AddSessionCommandService : ICommandService<AddSessionCommand>
{
	private readonly ISessionRepository _repository;

	public AddSessionCommandService(ISessionRepository repository)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}

	public ValueTask ExecuteAsync(AddSessionCommand command, CancellationToken cancellationToken = default)
		=> _repository.AddAsync(
			command.Token,
			command.BrandCode,
			command.PlayerCode,
			command.MerchantId,
			cancellationToken);
}
