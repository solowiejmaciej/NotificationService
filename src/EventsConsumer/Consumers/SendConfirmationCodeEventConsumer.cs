using EventsConsumer.Models.Body;
using MassTransit;
using Shared.Enums;
using Shared.Events;

namespace EventsConsumer.Consumers;

public class SendConfirmationCodeEventConsumer : IConsumer<SendConfirmationCodeEvent>
{
    private readonly ILogger<SendConfirmationCodeEventConsumer> _logger;
    private readonly INotificationApiClient _notificationApiClient;

    public SendConfirmationCodeEventConsumer(
        ILogger<SendConfirmationCodeEventConsumer> logger,
        INotificationApiClient notificationApiClient
        )
    {
        _logger = logger;
        _notificationApiClient = notificationApiClient;
    }
    public async Task Consume(ConsumeContext<SendConfirmationCodeEvent> context)
    {
        var confirmationCode = context.Message;
        switch (confirmationCode.NotificationChannel)
        {
            case ENotificationChannel.Email:
                _logger.LogInformation($"Sending email confirmation code{confirmationCode.Code}");
                _notificationApiClient.SendEmail(confirmationCode.UserId, 
                    new SendEmailBody($"Confirmation code is : {confirmationCode.Code}","ConfirmEmail"));
                break;
            case ENotificationChannel.SMS:
                _notificationApiClient.SendSms(confirmationCode.UserId, $"Code is {confirmationCode.Code}");
                _logger.LogInformation($"Sending sms confirmation code {confirmationCode.Code}");
                break;
            default:
                throw new InvalidOperationException("Invalid notification channel");
        }
        {
            
        }
    }
}