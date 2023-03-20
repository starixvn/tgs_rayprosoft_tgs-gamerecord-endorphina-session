using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class FailedSessionQueryUnitTest
{
    [Fact]
    public async Task 取得FailedSession()
    {
        var fakeFailedSessionRepository = Substitute.For<IFailedSessionRepository>();

        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";
        var merchantId = "1494";

        _ = fakeFailedSessionRepository.QueryFailedAsync(merchantId)
            .Returns(new Models.FailedSession[] {
                new Models.FailedSession("AA8BB60F5A134264BE838CF6EBA7C8F1",  brandCode, playerCode,  merchantId),
                new Models.FailedSession("B9EBE140F7294D44931200A37918F063",  brandCode, playerCode,  merchantId)
            }.ToAsyncEnumerable());

        var sut = new FailedSessionQueryService(fakeFailedSessionRepository);

        var actual = await sut
            .QueryAsync(new FailedSessionQuery(merchantId))
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Equal(2, actual!.Length);
    }
}
