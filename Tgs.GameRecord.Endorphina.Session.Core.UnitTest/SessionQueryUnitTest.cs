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

        var state = Common.TokenState.Queued;

        _ = fakeRepository.QueryAsync(state)
            .Returns(new Models.Session[] {
                new Models.Session("AA8BB60F5A134264BE838CF6EBA7C8F1", Common.TokenState.Queued, brandCode, playerCode, 0),
                new Models.Session("B9EBE140F7294D44931200A37918F063", Common.TokenState.Queued, brandCode, playerCode, 0)
            }.ToAsyncEnumerable());

        var sut = new SessionQueryService(fakeRepository);

        var actual = await sut
            .QueryAsync(new SessionQuery(state))
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Equal(2, actual!.Length);
    }
}
