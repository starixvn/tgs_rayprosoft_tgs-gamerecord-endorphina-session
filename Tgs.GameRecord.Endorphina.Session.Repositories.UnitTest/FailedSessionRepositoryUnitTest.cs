using MongoDB.Driver;
using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Repositories.Models;

namespace Tgs.GameRecord.Endorphina.Session.Repositories.UnitTest;

public class FailedSessionRepositoryUnitTest
{
	[Fact]
	public async Task AddFailed()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
		var brandCode = "GPI";
		var playerCode = "4ucmqplxb029";
		var merchantId = "1494";

		var repository = new FailedSessionRepository(fakerMongoDatabase);

		await repository.AddFailedAsync(token, brandCode, playerCode, merchantId);

		var failedSession = new FailedSessionModel()
		{
			Id = token,
			BrandCode = brandCode,
			PlayerCode = playerCode,
			MerchantId = merchantId,
		};

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<FailedSessionModel>("failed_sessions")
			.InsertOneAsync(
				document: Arg.Is(failedSession));
	}

	[Fact]
	public async Task Query()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new FailedSessionRepository(fakerMongoDatabase);

		var merchantId = "1494";
		var builder = Builders<FailedSessionModel>.Filter;
		var filter = builder.Eq(x => x.MerchantId, merchantId);

		var actual = await repository
			.QueryFailedAsync(merchantId)
			.ToArrayAsync();

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<FailedSessionModel>("failed_sessions")
			.FindAsync(
				filter: filter);

		Assert.NotNull(actual);
	}
}
