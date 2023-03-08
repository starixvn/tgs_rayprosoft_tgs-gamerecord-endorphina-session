using Grpc.Core;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;
using Tgs.GameRecord.Endorphina.Session.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.GrpcServices;

public class GameRecordEndorphinaSessionGrpcService : GameRecordEndorphinaSession.GameRecordEndorphinaSessionBase
{
    private readonly ICommandService<AddSessionCommand> _addService;
    private readonly ICommandService<SessionProcessingCommand> _processingService;
    private readonly ICommandService<SessionCompleteCommand> _completeService;
    private readonly ICommandService<SessionDeleteCommand> _deleteService;
    private readonly ICommandService<SessionFailedCommand> _failedService;
    private readonly IGetService<SessionGet, SessionInfo?> _getService;
    private readonly IQueryService<SessionQuery, SessionInfo> _queryService;
    private readonly ICommandService<UpdateSessionRetryCommand> _updateRetryService;
    private readonly ICommandService<MoveFailedSessionCommand> _moveFailedService;

    public GameRecordEndorphinaSessionGrpcService(
        ICommandService<AddSessionCommand> addService,
        ICommandService<SessionProcessingCommand> processingService,
        ICommandService<SessionCompleteCommand> completeService,
        ICommandService<SessionDeleteCommand> deleteService,
        ICommandService<SessionFailedCommand> failedService,
        IGetService<SessionGet, SessionInfo?> getService,
        IQueryService<SessionQuery, SessionInfo> queryService,
        ICommandService<UpdateSessionRetryCommand> updateRetryService,
        ICommandService<MoveFailedSessionCommand> moveFailedService)
    {
        _addService = addService ?? throw new ArgumentNullException(nameof(addService));
        _processingService = processingService ?? throw new ArgumentNullException(nameof(processingService));
        _completeService = completeService ?? throw new ArgumentNullException(nameof(completeService));
        _deleteService = deleteService ?? throw new ArgumentNullException(nameof(deleteService));
        _failedService = failedService ?? throw new ArgumentNullException(nameof(failedService));
        _getService = getService ?? throw new ArgumentNullException(nameof(getService));
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        _updateRetryService = updateRetryService ?? throw new ArgumentNullException(nameof(updateRetryService));
        _moveFailedService = moveFailedService ?? throw new ArgumentNullException(nameof(moveFailedService));
    }

    public override async Task<BaseReply> Add(AddRequest request, ServerCallContext context)
    {
        await _addService
            .ExecuteAsync(
                new AddSessionCommand(request.Token, request.BrandCode, request.PlayerCode),
                context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }

    public override async Task<BaseReply> Processing(ProcessingRequest request, ServerCallContext context)
    {
        await _processingService
            .ExecuteAsync(
                new SessionProcessingCommand(request.Token),
                context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }

    public override async Task<BaseReply> Complete(CompleteRequest request, ServerCallContext context)
    {
        await _completeService
            .ExecuteAsync(
                new SessionCompleteCommand(request.Token),
                context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }

    public override async Task<BaseReply> Remove(RemoveRequest request, ServerCallContext context)
    {
        await _deleteService
            .ExecuteAsync(
                new SessionDeleteCommand(request.Token),
                context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }

    public override async Task<BaseReply> Failed(FailedRequest request, ServerCallContext context)
    {
        await _failedService
            .ExecuteAsync(
                new SessionFailedCommand(request.Token),
                context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }

    public override async Task<Protos.Session> Get(GetRequest request, ServerCallContext context)
    {
        var session = await _getService
            .GetAsync(
                new SessionGet(request.Token),
                context.CancellationToken)
            .ConfigureAwait(false);

        if (session == null)
            return new Protos.Session();

        return new Protos.Session
        {
            Token = session!.Token,
            BrandCode = session.BrandCode,
            PlayerCode = session.PlayerCode,
            Retry = session.Retry,
            State = (TokenState)session.State
        };
    }

    public override async Task<SessionReply> Query(QueryRequest request, ServerCallContext context)
    {
        var sessions = await _queryService
            .QueryAsync(new SessionQuery((Common.TokenState)request.State), context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new SessionReply();

        if (sessions.Any())
        {
            foreach (var session in sessions)
            {
                reply.Items.Add(new Protos.Session()
                {
                    BrandCode = session.BrandCode,
                    PlayerCode = session.PlayerCode,
                    Retry = session.Retry,
                    State = (TokenState)session.State,
                    Token = session.Token,
                });
            }
        }
        return reply;
    }

    public override async Task<BaseReply> UpdateRetry(UpdateRetryRequest request, ServerCallContext context)
    {
        await _updateRetryService
            .ExecuteAsync(new UpdateSessionRetryCommand(request.Token))
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }

    public override async Task<BaseReply> MoveFailed(MoveFailedRequest request, ServerCallContext context)
    {
        await _moveFailedService
            .ExecuteAsync(new MoveFailedSessionCommand(request.Token))
            .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }
}
