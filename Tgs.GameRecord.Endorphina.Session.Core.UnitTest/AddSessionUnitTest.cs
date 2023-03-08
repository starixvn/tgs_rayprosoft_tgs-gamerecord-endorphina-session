using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;

namespace Tgs.GameRecord.Endorphina.Session.Core.UnitTest;

public class AddSessionUnitTestUnitTest
{
    [Fact]
    public async Task 新增Token()
    {
        var fakeRepository = Substitute.For<ISessionRepository>();

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";

        var sut = new AddSessionCommandService(fakeRepository);

        await sut.ExecuteAsync(new AddSessionCommand(token, brandCode, playerCode));

        _ = fakeRepository.Received(1)
            .AddAsync(token, brandCode, playerCode)
            .ConfigureAwait(false);
    }
}
