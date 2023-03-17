using MongoDB.Driver;
using Tgs.GameRecord.Endorphina.Session.Core;
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

    private IMongoCollection<FailedSessionModel> GetFailedSessionColletion()
                => _database.GetCollection<FailedSessionModel>("Endorphina_Failed_sessions");
}
