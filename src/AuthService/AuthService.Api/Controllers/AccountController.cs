using AuthService.Api.Models.QueryParameters.ConfirmationCode;
using AuthService.Application.MediatR.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace AuthService.Api.Controllers
{
    //[EnableCors("apiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        [HttpPost("SendSmsConfirmationCode")]
        public async Task<ActionResult> SendSmsConfirmationCode()
        {
            await _mediator.Send(new SendConfirmationCodeCommand()
            {
                NotificationChannel = ENotificationChannel.SMS,
            });
            return Accepted();
        }
        
        [HttpHead("VerifySmsConfirmationCode")]
        public async Task<ActionResult> VerifySmsConfirmationCode(
            [FromQuery] VerifyCodeQueryParams queryParams
            )
        {
            await _mediator.Send(new VerifyConfirmationCodeCommand()
            {
                NotificationChannel = ENotificationChannel.SMS,
                Code = queryParams.Code
            });
            return Accepted();
        }
        
        [HttpPost("SendEmailConfirmationCode")]
        public async Task<ActionResult> SendEmailConfirmationCode()
        {
            await _mediator.Send(new SendConfirmationCodeCommand()
            {
                NotificationChannel = ENotificationChannel.Email,
            });
            return Accepted();
        }
        
        [HttpHead("VerifyEmailConfirmationCode")]
        public async Task<ActionResult> VerifyEmailConfirmationCode(
            [FromQuery] VerifyCodeQueryParams queryParams
            )
        {
            await _mediator.Send(new VerifyConfirmationCodeCommand()
            {
                NotificationChannel = ENotificationChannel.Email,
                Code = queryParams.Code
            });
            return Accepted();
        }

    }
}