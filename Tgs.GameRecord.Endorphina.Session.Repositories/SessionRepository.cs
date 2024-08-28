using System.Runtime.CompilerServices;
using MongoDB.Driver;
using Tgs.GameRecord.Endorphina.Session.Core;
using Tgs.GameRecord.Endorphina.Session.Repositories.Extensions;
using Tgs.GameRecord.Endorphina.Session.Repositories.Models;

namespace Tgs.GameRecord.Endorphina.Session.Repositories;

internal class SessionRepository : ISessionRepository
{
	private readonly IMongoDatabase _database;

	public SessionRepository(
		IMongoDatabase database)
	{
		_database = database ?? throw new ArgumentNullException(nameof(database));
	}

	public async ValueTask InitDBAsync(CancellationToken cancellationToken = default)
	{
		var sessionCollection = GetCollection();

		await State_IndexAsync(sessionCollection, cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask AddAsync(string token, string brandCode, string playerCode, string merchantId, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var document = new SessionModel()
		{
			Id = token,
			BrandCode = brandCode,
			PlayerCode = playerCode,
			State = TokenState.Queued,
			MerchantId = merchantId,
			Retry = 0,
			CreatedOn = DateTime.UtcNow,
			UpdateOn = DateTime.UtcNow

		};

		await collection
			.InsertOneAsync(
				document: document,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async IAsyncEnumerable<Core.Models.Session> QueryAsync(
		TokenState state,
		string merchantId,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.State, state)
			& builder.Eq(x => x.MerchantId, merchantId);

		var asyncCursor = await collection
			.FindAsync(
				filter: filter,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		while (await asyncCursor
				.MoveNextAsync(
					cancellationToken: cancellationToken)
				.ConfigureAwait(false))
		{
			foreach (var current in asyncCursor.Current)
			{
				yield return new Core.Models.Session(
					Token: current.Id,
					State: current.State,
					BrandCode: current.BrandCode,
					PlayerCode: current.PlayerCode,
					Retry: current.Retry,
					MerchantId: current.MerchantId);
			}
		}
	}

	public async ValueTask UpdateProcessingAsync(string token, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.Id, token);

		var update = Builders<SessionModel>.Update
			.Set(s => s.State, TokenState.Processing)
			.Set(s => s.UpdateOn, DateTime.UtcNow);

		_ = await collection
			.UpdateOneAsync(
				filter: filter,
				update: update,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask UpdateRetryAsync(string token, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.Id, token);

		var update = Builders<SessionModel>.Update
			.Inc(s => s.Retry, 1)
			.Set(s => s.State, TokenState.Queued)
			.Set(s => s.UpdateOn, DateTime.UtcNow);

		var result = await collection
			.UpdateOneAsync(
				filter: filter,
				update: update,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask UpdateCompleteAsync(string token, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.Id, token);

		var update = Builders<SessionModel>.Update
			.Set(s => s.State, TokenState.Complete)
			.Set(s => s.UpdateOn, DateTime.UtcNow);

		_ = await collection
			.UpdateOneAsync(
				filter: filter,
				update: update,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask UpdateFailedAsync(string token, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.Id, token);

		var update = Builders<SessionModel>.Update
			.Set(s => s.State, TokenState.Failed)
			.Set(s => s.UpdateOn, DateTime.UtcNow);

		_ = await collection
			.UpdateOneAsync(
				filter: filter,
				update: update,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask DeleteAsync(string token, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.Id, token);

		_ = await collection
			.DeleteOneAsync(
				filter: filter,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<Core.Models.Session?> GetAsync(string token, CancellationToken cancellationToken = default)
	{
		var collection = GetCollection();

		var builder = Builders<SessionModel>.Filter;

		var filter = builder.Eq(x => x.Id, token);

		var asyncCursor = await collection
			.FindAsync(
				filter: filter,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var result = asyncCursor.FirstOrDefault(cancellationToken: cancellationToken);

		return result == null
			? null
			: new Core.Models.Session(
				result.Id,
				result.State,
				result.BrandCode,
				result.PlayerCode,
				result.Retry,
				result.MerchantId);
	}

	private static Task State_IndexAsync(
		IMongoCollection<SessionModel> collection,
		CancellationToken cancellationToken = default)
	{
		var index = Builders<SessionModel>.IndexKeys
			.Ascending(m => m.State);

		var createIndexOptions = new CreateIndexOptions
		{
			Name = $"ix_state"
		};
		var createIndexModel = new CreateIndexModel<SessionModel>(index, createIndexOptions);

		return collection.Indexes.CreateOneAsync(
			model: createIndexModel,
			cancellationToken: cancellationToken);
	}

	private IMongoCollection<SessionModel> GetCollection()
		=> _database.GetCollection<SessionModel>(typeof(SessionModel).GetTableName());
}
