using AuthService.Application.ApplicationUserContext;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace AuthService.Application.MediatR.Command;

public class SendConfirmationCodeCommandHandler : IRequestHandler<SendConfirmationCodeCommand>
{
    private readonly ILogger<SendConfirmationCodeCommandHandler> _logger;
    private readonly IConfirmationCodesRepository _confirmationCodesRepository;
    private readonly IUserContext _userContext;
    private readonly IEventPublisher _eventPublisher;

    public SendConfirmationCodeCommandHandler(
        ILogger<SendConfirmationCodeCommandHandler> logger,
        IConfirmationCodesRepository confirmationCodesRepository,
        IUserContext userContext,
        IEventPublisher eventPublisher
        )
    {
        _logger = logger;
        _confirmationCodesRepository = confirmationCodesRepository;
        _userContext = userContext;
        _eventPublisher = eventPublisher;
    }
    public async Task Handle(SendConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        switch (request.NotificationChannel)
        {
            case ENotificationChannel.Email:
                var emailCode = await _confirmationCodesRepository.GenerateCodeAsync(ENotificationChannel.Email, currentUser.Id);
                _logger.LogInformation("Sending email confirmation code, {}", emailCode.Code);
                await _eventPublisher.PublishSendConfirmationCodeEventAsync(emailCode, currentUser.Id, cancellationToken);
                break;
            case ENotificationChannel.SMS:
                var smsCode = await _confirmationCodesRepository.GenerateCodeAsync(ENotificationChannel.SMS, currentUser.Id);
                _logger.LogInformation("Sending sms confirmation code, {}", smsCode.Code);
                await _eventPublisher.PublishSendConfirmationCodeEventAsync(smsCode, currentUser.Id, cancellationToken);
                break;
            default:
                throw new InvalidOperationException("Invalid notification channel");
        }
    }
}


public class SendConfirmationCodeCommand : IRequest
{
    public ENotificationChannel NotificationChannel { get; set; }
}