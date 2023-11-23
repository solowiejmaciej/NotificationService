#region

using AuthService.Application.ApplicationUserContext;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using Shared.Exceptions;

#endregion

namespace AuthService.Application.MediatR.Command;

public class VerifyConfirmationCodeCommandHandler : IRequestHandler<VerifyConfirmationCodeCommand>
{
    private readonly ILogger<VerifyConfirmationCodeCommandHandler> _logger;
    private readonly IConfirmationCodesRepository _confirmationCodesRepository;
    private readonly IUserContext _userContext;
    private readonly IUsersRepository _usersRepository;

    public VerifyConfirmationCodeCommandHandler(
        ILogger<VerifyConfirmationCodeCommandHandler> logger,
        IConfirmationCodesRepository confirmationCodesRepository,
        IUserContext userContext,
        IUsersRepository usersRepository
    )
    {
        _logger = logger;
        _confirmationCodesRepository = confirmationCodesRepository;
        _userContext = userContext;
        _usersRepository = usersRepository;
    }

    public async Task Handle(VerifyConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        switch (request.NotificationChannel)
        {
            case ENotificationChannel.Email:
                _logger.LogInformation("Verifying email confirmation code");
                var isEmailCodeValid =
                    await _confirmationCodesRepository.IsValidCode(currentUser.Id, request.Code,
                        ENotificationChannel.Email);
                if (!isEmailCodeValid) throw new NotFoundException("Invalid code");
                await _usersRepository.ConfirmEmailAsync(currentUser.Id);
                break;
            case ENotificationChannel.SMS:
                _logger.LogInformation("Verifying sms confirmation code");
                var isSmsCodeValid =
                    await _confirmationCodesRepository.IsValidCode(currentUser.Id, request.Code,
                        ENotificationChannel.SMS);
                if (!isSmsCodeValid) throw new NotFoundException("Invalid code");
                await _usersRepository.ConfirmPhoneNumberAsync(currentUser.Id, cancellationToken);
                break;
            default:
                throw new NotFoundException("Invalid notification channel");
        }
    }
}

public class VerifyConfirmationCodeCommand : IRequest
{
    public ENotificationChannel NotificationChannel { get; set; }
    public string Code { get; set; }
}