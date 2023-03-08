using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class SessionGetUnitTest
{
    [Fact]
    public async Task 取Token()
    {
        var fakeRepository = Substitute.For<ISessionRepository>();

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";

        _ = fakeRepository.GetAsync(token)
            .Returns(new Models.Session(token, TokenState.Queued, brandCode, playerCode, 0));

        var sut = new SessionGetService(fakeRepository);

        var actual = await sut.GetAsync(new SessionGet(token));

        Assert.Equal(token, actual!.Token);
        Assert.Equal(brandCode, actual!.BrandCode);
        Assert.Equal(playerCode, actual!.PlayerCode);
    }
}
