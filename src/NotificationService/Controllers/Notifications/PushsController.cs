#region

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.MediatR.Handlers.CreateNew;
using NotificationService.MediatR.Handlers.Delete;
using NotificationService.MediatR.Handlers.GetAll;
using NotificationService.MediatR.Handlers.GetById;
using NotificationService.Models.QueryParameters.Create;
using NotificationService.Models.QueryParameters.GetAll;
using NotificationService.Models.Requests;

#endregion

namespace NotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PushsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PushsController(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Add(
        [FromBody] AddPushRequest request,
        [FromQuery] PushRequestQuerryParameters parameters
    )
    {
        var command = new CreateNewPushCommand
        {
            Content = request.Content,
            Title = request.Title,
            RecipiantId = parameters.UserId
        };
        var createdPushId = await _mediator.Send(command);
        return Created($"/api/Push/{createdPushId}", command);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetAllPushesRequestQueryParameters queryParameters
    )
    {
        var query = new GetAllPushesQuery
        {
            SearchPhrase = queryParameters.SearchPhrase,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            Status = queryParameters.Status
        };
        var allPushesDtoByCurrentUser = await _mediator.Send(query);
        return Ok(allPushesDtoByCurrentUser);
    }

    [Authorize]
    [Route("{id:int}")]
    [HttpGet]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var query = new GetPushByIdQuerry
        {
            Id = id
        };

        var pushCreatedByCurrentUserWithSearchId = await _mediator.Send(query);
        return Ok(pushCreatedByCurrentUserWithSearchId);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeletePushCommand
        {
            Id = id
        };
        await _mediator.Send(command);
        return Accepted();
    }
}