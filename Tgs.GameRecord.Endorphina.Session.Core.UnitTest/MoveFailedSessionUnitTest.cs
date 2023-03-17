using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class MoveFailedSessionUnitTest
{
    [Fact]
    public async Task 搬移狀態Failed且Retry3次的Session到FailedSession表()
    {
        var fakeRepository = Substitute.For<ISessionRepository>();
        var fakeFailedSessionRepository = Substitute.For<IFailedSessionRepository>();

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";
        var merchantId = "1494";

        _ = fakeRepository
            .GetAsync(token)
            .Returns(new Models.Session(token, TokenState.Failed, brandCode, playerCode, 3, merchantId));

        var sut = new MoveFailedSessionCommandService(fakeRepository, fakeFailedSessionRepository);

        await sut.ExecuteAsync(new MoveFailedSessionCommand(token));

        _ = fakeFailedSessionRepository.Received(1)
            .AddFailedAsync(token, brandCode, playerCode, merchantId)
            .ConfigureAwait(false);

        _ = fakeRepository.Received(1)
            .DeleteAsync(token)
            .ConfigureAwait(false);
    }
}
