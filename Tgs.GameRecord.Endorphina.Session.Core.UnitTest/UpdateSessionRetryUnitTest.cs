using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class UpdateSessionRetryUnitTest
{
	[Fact]
	public async Task 更新Token的Retry次數()
	{
		var fakeRepository = Substitute.For<ISessionRepository>();

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		var sut = new UpdateSessionRetryCommandService(fakeRepository);

		await sut.ExecuteAsync(new UpdateSessionRetryCommand(token));

		_ = fakeRepository
			.Received(1)
			.UpdateRetryAsync(token);
	}
}
