using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class SessionDeleteUnitTestUnitTest
{
    [Fact]
    public async Task 刪除Token()
    {
        var fakeRepository = Substitute.For<ISessionRepository>();

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        var sut = new SessionDeleteCommandService(fakeRepository);

        await sut.ExecuteAsync(new SessionDeleteCommand(token));

        _ = fakeRepository.Received(1)
            .DeleteAsync(token)
            .ConfigureAwait(false);
    }
}
