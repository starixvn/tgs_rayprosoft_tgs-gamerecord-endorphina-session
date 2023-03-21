using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.Endorphina.Session.Core;
using Tgs.GameRecord.Endorphina.Session.Core.ApplicationServices;
using Tgs.GameRecord.Endorphina.Session.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Endorphina.Session.Controllers;

[Route("[controller]")]
[ApiController]
public class SessionController : ControllerBase
{
    /// <summary>
    /// Adds the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1",
    ///             "brandCode": "GPI",
    ///             "playerCode": "4ucmqplxb029",
    ///             "merchantId": "1494"
    ///         }
    /// </remarks>
    [HttpPost("add", Name = "Add")]
    public ValueTask AddAsync(
        [FromServices] ICommandService<AddSessionCommand> service,
        [FromBody] AddSessionViewModel model)
        => service.ExecuteAsync(
            command: new AddSessionCommand(
                model.Token,
                model.BrandCode,
                model.PlayerCode,
                model.MerchantId),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Get
    ///         /AA8BB60F5A134264BE838CF6EBA7C8F1    
    /// </remarks>
    [HttpGet("get/{token}", Name = "Get")]
    public async ValueTask<SessionInfoViewModel?> GetAsync(
        [FromServices] IGetService<SessionGet, SessionInfo?> service,
        string token)
    {
        var result = await service
            .GetAsync(new SessionGet(token), HttpContext.RequestAborted)
            .ConfigureAwait(false);

        if (result is null)
            return null;

        return new SessionInfoViewModel(
            result.Token,
            result.State,
            result.BrandCode,
            result.PlayerCode,
            result.Retry,
            result.MerchantId);
    }

    /// <summary>
    /// Queries the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="state">The state.</param>
    /// <param name="merchantId">The merchantId.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Get
    ///         /Queued
    ///         /1494
    /// </remarks>
    [HttpGet("query/{merchantId}/{state}", Name = "Query")]
    public IAsyncEnumerable<SessionInfo> QueryAsync(
        [FromServices] IQueryService<SessionQuery, SessionInfo> service,
        TokenState state,
        string merchantId)
        => service
        .QueryAsync(
            query: new SessionQuery(
                State: state,
                MerchantId: merchantId),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Completes the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Put
    ///         AA8BB60F5A134264BE838CF6EBA7C8F1
    /// </remarks>
    [HttpPut("{token}/complete", Name = "Complete")]
    public ValueTask CompleteAsync(
        [FromServices] ICommandService<SessionCompleteCommand> service,
        string token)
        => service
        .ExecuteAsync(
            command: new SessionCompleteCommand(
                Token: token),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Faileds the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Put
    ///         AA8BB60F5A134264BE838CF6EBA7C8F1
    /// </remarks>
    [HttpPut("{token}/failed", Name = "Failed")]
    public ValueTask FailedAsync(
        [FromServices] ICommandService<SessionFailedCommand> service,
        string token)
        => service
        .ExecuteAsync(
            command: new SessionFailedCommand(
                Token: token),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Processings the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Put
    ///         AA8BB60F5A134264BE838CF6EBA7C8F1
    /// </remarks>
    [HttpPut("{token}/processing", Name = "Processing")]
    public ValueTask ProcessingAsync(
        [FromServices] ICommandService<SessionProcessingCommand> service,
        string token)
        => service
        .ExecuteAsync(
            command: new SessionProcessingCommand(
                Token: token),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Updates the retry asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Put
    ///         AA8BB60F5A134264BE838CF6EBA7C8F1
    /// </remarks>
    [HttpPut("{token}/updateRetry", Name = "UpdateRetry")]
    public ValueTask UpdateRetryAsync(
        [FromServices] ICommandService<UpdateSessionRetryCommand> service,
        string token)
        => service
        .ExecuteAsync(
            command: new UpdateSessionRetryCommand(token),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Delete
    ///         AA8BB60F5A134264BE838CF6EBA7C8F1
    /// </remarks>
    [HttpDelete("delete/{token}")]
    public ValueTask DeleteAsync(
        [FromServices] ICommandService<SessionDeleteCommand> service,
        string token)
        => service.ExecuteAsync(
            command: new SessionDeleteCommand(token),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// 從Session表 搬移到 Failed Session表.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Put
    ///         AA8BB60F5A134264BE838CF6EBA7C8F1
    /// </remarks>
    [HttpPut("{token}/moveFailed", Name = "MoveFailed")]
    public ValueTask MoveFailedAsync(
        [FromServices] ICommandService<MoveFailedSessionCommand> service,
        string token)
        => service.ExecuteAsync(
            command: new MoveFailedSessionCommand(token),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// 查詢Failed Session
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="merchantId">The merchant identifier.</param>
    /// <returns></returns>
    [HttpGet("query/{merchantId}", Name = "QueryFailed")]
    public IAsyncEnumerable<FailedSessionInfo> QueryFailedAsync(
        [FromServices] IQueryService<FailedSessionQuery, FailedSessionInfo> service,
        string merchantId)
        => service
        .QueryAsync(
            query: new FailedSessionQuery(
                MerchantId: merchantId),
            cancellationToken: HttpContext.RequestAborted);
}
