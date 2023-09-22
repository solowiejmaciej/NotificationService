
using System.Threading.Tasks;
using AuthService.Api.Models.QueryParameters.GetAll;
using AuthService.Application.MediatR.Command;
using AuthService.Application.MediatR.Query;
using AuthService.Models.Requests.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [EnableCors("apiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] GetAllUsersQueryParameters queryParameters
            )
        {
            var query = new GetAllUsersQuery()
            {
                SearchPhrase = queryParameters.SearchPhrase,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute] string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery()
            {
                Id = id
            });
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(
            [FromRoute] string id,
            [FromBody] UpdateUserRequest request
            )
        {
            var updateUserCommand = new UpdateUserCommand()
            {
                Id = id,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DeviceId = request.DeviceId,
                Firstname = request.Firstname,
                Surname = request.Surname,
            };

            var updatedUserDto = await _mediator.Send(updateUserCommand);
            return Ok(updatedUserDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand()
            {
                Id = id,
            });
            return Accepted();
        }
    }
}