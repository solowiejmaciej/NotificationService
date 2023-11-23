#region

using AuthService.Api.Models.Requests.Add;
using AuthService.Api.Models.Requests.Login;
using AuthService.Application.MediatR.Command;
using AuthService.Application.MediatR.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace AuthService.Api.Controllers;

//[EnableCors("apiCorsPolicy")]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(AddUserRequest user)
    {
        var command = new CreateNewUserCommand
        {
            Password = user.Password,
            ConfirmPassword = user.ConfirmPassword,
            DeviceId = user.DeviceId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Firstname = user.Firstname,
            Surname = user.Surname
        };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login(UserLoginRequest user)
    {
        var response = await _mediator.Send(new GetTokenQuery
        {
            Email = user.Email,
            Password = user.Password
        });
        return Ok(response);
    }

    [HttpPost("Login/QR")]
    public ActionResult LoginViaQr()
    {
        throw new NotImplementedException();
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest refreshTokenRequest
    )
    {
        var response = await _mediator.Send(new RefreshTokenCommand
        {
            Token = refreshTokenRequest.Token,
            RefreshToken = refreshTokenRequest.RefreshToken
        });
        return Ok(response);
    }
}