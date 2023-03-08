using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class SessionProcessingUnitTest
{
    [Fact]
    public async Task 更新Token狀態為Processing()
    {
        var fakeRepository = Substitute.For<ISessionRepository>();

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        var sut = new SessionProcessingCommandService(fakeRepository);

        await sut.ExecuteAsync(new SessionProcessingCommand(token));

        _ = fakeRepository.Received(1)
            .UpdateProcessingAsync(token)
            .ConfigureAwait(false);
    }
}
