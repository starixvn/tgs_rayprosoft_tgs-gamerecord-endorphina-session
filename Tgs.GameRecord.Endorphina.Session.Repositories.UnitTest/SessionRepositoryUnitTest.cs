using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using NSubstitute;
using Tgs.GameRecord.Endorphina.Session.Common;
using Tgs.GameRecord.Endorphina.Session.Repositories.Models;

namespace Tgs.GameRecord.Endorphina.Session.Repositories.UnitTest;

public class SessionRepositoryUnitTest
{
    [Fact]
    public async Task Add()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";

        await repository
            .AddAsync(token, brandCode, playerCode)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);
        var update = Builders<SessionModel>.Update
            .Set(s => s.State, TokenState.Complete);

        var session = new SessionModel()
        {
            Id = token,
            BrandCode = brandCode,
            PlayerCode = playerCode,
            State = TokenState.Queued,
            Retry = 0
        };

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .InsertOneAsync(
                document: Arg.Is(session))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Query()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var state = TokenState.Queued;
        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.State, state);

        var actual = await repository
            .QueryAsync(state)
            .ToArrayAsync()
            .ConfigureAwait(false);

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .FindAsync(
                filter: filter)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Length >= 0);
    }

    [Fact]
    public async Task UpdateProcessing()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        await repository
            .UpdateCompleteAsync(token)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);
        var update = Builders<SessionModel>.Update
            .Set(s => s.State, TokenState.Processing);

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .UpdateOneAsync(
                filter: Arg.Is(filter),
                update: Arg.Is(update))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task UpdateRetry()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        await repository
            .UpdateRetryAsync(token)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);
        var update = Builders<SessionModel>.Update
            .Inc(s => s.Retry, 1);

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .UpdateOneAsync(
                filter: Arg.Is(filter),
                update: Arg.Is(update))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task UpdateComplete()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        await repository
            .UpdateCompleteAsync(token)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);
        var update = Builders<SessionModel>.Update
            .Set(s => s.State, TokenState.Complete);

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .UpdateOneAsync(
                filter: Arg.Is(filter),
                update: Arg.Is(update))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task UpdateFailed()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        await repository
            .UpdateCompleteAsync(token)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);
        var update = Builders<SessionModel>.Update
            .Set(s => s.State, TokenState.Failed);

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .UpdateOneAsync(
                filter: Arg.Is(filter),
                update: Arg.Is(update))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Delete()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        await repository
            .DeleteAsync(token)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<SessionModel>("Endorphina_sessions")
            .DeleteOneAsync(
                filter: Arg.Is(filter))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task AddFailed()
    {
        var fakeLogger = NullLogger<SessionRepository>.Instance;
        var fakerMongoDatabase = Substitute.For<IMongoDatabase>();

        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";
        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";

        var repository = new SessionRepository(fakeLogger, fakerMongoDatabase);

        await repository
            .AddFailedAsync(token, brandCode, playerCode)
            .ConfigureAwait(false);

        var builder = Builders<SessionModel>.Filter;
        var filter = builder.Eq(x => x.Id, token);
        var update = Builders<SessionModel>.Update
            .Set(s => s.State, TokenState.Complete);

        var failedSession = new FailedSessionModel()
        {
            Id = token,
            BrandCode = brandCode,
            PlayerCode = playerCode,
        };

        _ = fakerMongoDatabase.Received(1)
            .GetCollection<FailedSessionModel>("Endorphina_Failed_sessions")
            .InsertOneAsync(
                document: Arg.Is(failedSession))
            .ConfigureAwait(false);
    }
}
