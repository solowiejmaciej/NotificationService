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
using NotificationService.Models.Requests.Add;

namespace NotificationService.Controllers.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmsesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] AddSmsRequest request,
            [FromQuery] CreateSmsRequestQueryParameters parameters
        )
        {
            var command = new CreateNewSmsCommand()
            {
                Content = request.Content,
                RecipiantId = parameters.UserId
            };

            var createdSmsId = await _mediator.Send(command);
            return Created($"/api/Sms/{createdSmsId}", command);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] GetAllSmsRequestQueryParameters queryParameters
        )
        {
            var query = new GetAllPushesQuery()
            {
                SearchPhrase = queryParameters.SearchPhrase,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                Status = queryParameters.Status
            };
            var allSmsDtoByCurrentUser = await _mediator.Send(query);
            return Ok(allSmsDtoByCurrentUser);
        }

        [Authorize]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var query = new GetSmsByIdQuerry()
            {
                Id = id
            };

            var smsCreatedByCurrentUserWithSearchId = await _mediator.Send(query);
            return Ok(smsCreatedByCurrentUserWithSearchId);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteSmsCommand()
            {
                Id = id
            };
            await _mediator.Send(command);
            return Accepted();
        }
    }
}