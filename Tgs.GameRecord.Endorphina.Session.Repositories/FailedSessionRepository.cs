using System.Runtime.CompilerServices;
using MongoDB.Driver;
using Tgs.GameRecord.Endorphina.Session.Core;
using Tgs.GameRecord.Endorphina.Session.Core.Models;
using Tgs.GameRecord.Endorphina.Session.Repositories.Models;

namespace Tgs.GameRecord.Endorphina.Session.Repositories;

internal class FailedSessionRepository : IFailedSessionRepository
{
    private readonly IMongoDatabase _database;

    public FailedSessionRepository(
        IMongoDatabase database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async ValueTask InitDBAsync(CancellationToken cancellationToken = default)
    {
        var failedSessionCollection = GetFailedSessionColletion();

        await MerchantId_IndexAsync(failedSessionCollection, cancellationToken)
            .ConfigureAwait(false);

        await BrandCode_IndexAsync(failedSessionCollection, cancellationToken)
            .ConfigureAwait(false);

        await PlayerCode_IndexAsync(failedSessionCollection, cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask AddFailedAsync(string token, string brandCode, string playerCode, string merchantId, CancellationToken cancellationToken = default)
    {
        var collection = GetFailedSessionColletion();

        var document = new FailedSessionModel()
        {
            Id = token,
            BrandCode = brandCode,
            PlayerCode = playerCode,
            MerchantId = merchantId,
        };

        await collection
            .InsertOneAsync(
                document: document,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<FailedSession> QueryFailedAsync(
        string merchantId,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var collection = GetFailedSessionColletion();

        var builder = Builders<FailedSessionModel>.Filter;

        var filter = builder.Eq(x => x.MerchantId, merchantId);

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
                yield return new Core.Models.FailedSession(
                    Token: current.Id,
                    BrandCode: current.BrandCode,
                    PlayerCode: current.PlayerCode,
                    MerchantId: current.MerchantId);
            }
        }
    }

    private static Task BrandCode_IndexAsync(
        IMongoCollection<FailedSessionModel> collection,
        CancellationToken cancellationToken = default)
    {
        var index = Builders<FailedSessionModel>.IndexKeys
            .Ascending(m => m.BrandCode);

        var createIndexOptions = new CreateIndexOptions
        {
            Name = $"ix_brand_code"
        };
        var createIndexModel = new CreateIndexModel<FailedSessionModel>(index, createIndexOptions);

        return collection.Indexes.CreateOneAsync(
            model: createIndexModel,
            cancellationToken: cancellationToken);
    }

    private static Task PlayerCode_IndexAsync(
        IMongoCollection<FailedSessionModel> collection,
        CancellationToken cancellationToken = default)
    {
        var index = Builders<FailedSessionModel>.IndexKeys
            .Ascending(m => m.PlayerCode);

        var createIndexOptions = new CreateIndexOptions
        {
            Name = $"ix_player_code"
        };
        var createIndexModel = new CreateIndexModel<FailedSessionModel>(index, createIndexOptions);

        return collection.Indexes.CreateOneAsync(
            model: createIndexModel,
            cancellationToken: cancellationToken);
    }

    private static Task MerchantId_IndexAsync(
        IMongoCollection<FailedSessionModel> collection,
        CancellationToken cancellationToken = default)
    {
        var index = Builders<FailedSessionModel>.IndexKeys
            .Ascending(m => m.MerchantId);

        var createIndexOptions = new CreateIndexOptions
        {
            Name = $"ix_merchant_id"
        };
        var createIndexModel = new CreateIndexModel<FailedSessionModel>(index, createIndexOptions);

        return collection.Indexes.CreateOneAsync(
            model: createIndexModel,
            cancellationToken: cancellationToken);
    }

    private IMongoCollection<FailedSessionModel> GetFailedSessionColletion()
                => _database.GetCollection<FailedSessionModel>("Endorphina_Failed_sessions");
}
