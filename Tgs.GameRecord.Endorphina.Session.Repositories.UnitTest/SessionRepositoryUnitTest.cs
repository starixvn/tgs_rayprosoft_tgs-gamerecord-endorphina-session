using MongoDB.Driver;
using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Core;
using Tgs.GameRecord.Endorphina.Session.Repositories.Models;

namespace Tgs.GameRecord.Endorphina.Session.Repositories.UnitTest;

public class SessionRepositoryUnitTest
{
	[Fact]
	public async Task Add()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
		var brandCode = "GPI";
		var playerCode = "4ucmqplxb029";
		var merchantId = "1494";

		await repository.AddAsync(token, brandCode, playerCode, merchantId);

		var session = new SessionModel()
		{
			Id = token,
			BrandCode = brandCode,
			PlayerCode = playerCode,
			State = TokenState.Queued,
			Retry = 0
		};

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.InsertOneAsync(
				document: Arg.Is(session));
	}

	[Fact]
	public async Task Query()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var state = TokenState.Queued;
		var merchantId = "1494";
		var builder = Builders<SessionModel>.Filter;
		var filter = builder.Eq(x => x.State, state)
			& builder.Eq(x => x.MerchantId, merchantId);

		var actual = await repository
			.QueryAsync(state, merchantId)
			.ToArrayAsync();

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.FindAsync(filter: filter);

		Assert.NotNull(actual);
	}

	[Fact]
	public async Task UpdateProcessing()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		await repository.UpdateCompleteAsync(token);

		var builder = Builders<SessionModel>.Filter;
		var filter = builder.Eq(x => x.Id, token);
		var update = Builders<SessionModel>.Update
			.Set(s => s.State, TokenState.Processing);

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.UpdateOneAsync(
				filter: Arg.Is(filter),
				update: Arg.Is(update));
	}

	[Fact]
	public async Task UpdateRetry()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		await repository.UpdateRetryAsync(token);

		var builder = Builders<SessionModel>.Filter;
		var filter = builder.Eq(x => x.Id, token);
		var update = Builders<SessionModel>.Update
			.Inc(s => s.Retry, 1)
			.Set(s => s.State, TokenState.Queued);

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.UpdateOneAsync(
				filter: Arg.Is(filter),
				update: Arg.Is(update));
	}

	[Fact]
	public async Task UpdateComplete()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		await repository.UpdateCompleteAsync(token);

		var builder = Builders<SessionModel>.Filter;
		var filter = builder.Eq(x => x.Id, token);
		var update = Builders<SessionModel>.Update
			.Set(s => s.State, TokenState.Complete);

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.UpdateOneAsync(
				filter: Arg.Is(filter),
				update: Arg.Is(update));
	}

	[Fact]
	public async Task UpdateFailed()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		await repository.UpdateCompleteAsync(token);

		var builder = Builders<SessionModel>.Filter;
		var filter = builder.Eq(x => x.Id, token);
		var update = Builders<SessionModel>.Update
			.Set(s => s.State, TokenState.Failed);

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.UpdateOneAsync(
				filter: Arg.Is(filter),
				update: Arg.Is(update));
	}

	[Fact]
	public async Task Delete()
	{
		var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

		var repository = new SessionRepository(fakerMongoDatabase);

		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		await repository.DeleteAsync(token);

		var builder = Builders<SessionModel>.Filter;
		var filter = builder.Eq(x => x.Id, token);

		_ = fakerMongoDatabase
			.Received(1)
			.GetCollection<SessionModel>("session_forms")
			.DeleteOneAsync(
				filter: Arg.Is(filter));
	}
}
