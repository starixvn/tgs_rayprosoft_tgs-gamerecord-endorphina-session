using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class SessionCompleteUnitTest
{
	[Fact]
	public async Task 更新Token狀態為Complete()
	{
		var fakeRepository = Substitute.For<ISessionRepository>();

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		var sut = new SessionCompleteCommandService(fakeRepository);

		await sut.ExecuteAsync(new SessionCompleteCommand(token));

		_ = fakeRepository
			.Received(1)
			.UpdateCompleteAsync(token);
	}
}
