using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class SessionQueryUnitTest
{
	[Fact]
	public async Task 取Tokens()
	{
		var fakeRepository = Substitute.For<ISessionRepository>();

		var brandCode = "GPI";
		var playerCode = "4ucmqplxb029";
		var merchantId = "1494";

		var state = TokenState.Queued;

		_ = fakeRepository.QueryAsync(state, merchantId)
			.Returns(new Models.Session[] {
				new("AA8BB60F5A134264BE838CF6EBA7C8F1", TokenState.Queued, brandCode, playerCode, 0, merchantId),
				new("B9EBE140F7294D44931200A37918F063", TokenState.Queued, brandCode, playerCode, 0, merchantId)
			}.ToAsyncEnumerable());

		var sut = new SessionQueryService(fakeRepository);

		var actual = await sut
			.QueryAsync(new SessionQuery(state, merchantId))
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal(2, actual!.Length);
	}
}
