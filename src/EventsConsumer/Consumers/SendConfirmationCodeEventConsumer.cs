using EventsConsumer.Events;
using EventsConsumer.Managers;
using EventsConsumer.Models.Body;
using MassTransit;
using Shared.Enums;
using Shared.Events;

namespace EventsConsumer.Consumers;

public class SendConfirmationCodeEventConsumer : IConsumer<SendConfirmationCodeEvent>
{
    private readonly ILogger<SendConfirmationCodeEventConsumer> _logger;
    private readonly INotificationApiClient _notificationApiClient;
    private readonly IEventManager<NotificationEvent> _eventManager;

    public SendConfirmationCodeEventConsumer(
        ILogger<SendConfirmationCodeEventConsumer> logger,
        INotificationApiClient notificationApiClient,
        IEventManager<NotificationEvent> eventManager
            )
    {
        _logger = logger;
        _notificationApiClient = notificationApiClient;
        _eventManager = eventManager;
    }
    public async Task Consume(ConsumeContext<SendConfirmationCodeEvent> context)
    {
        var confirmationCodeEvent = context.Message;
        await _eventManager.AddAsync(confirmationCodeEvent);
        switch (confirmationCodeEvent.NotificationChannel)
        {
            case ENotificationChannel.Email:
                await _eventManager.ChangeStatusAsync(confirmationCodeEvent, EStatus.Processing);
                _logger.LogInformation($"Sending email confirmation code{confirmationCodeEvent.Code}");
                _notificationApiClient.SendEmail(confirmationCodeEvent.UserId, new SendEmailBody($"Confirmation code is : {confirmationCodeEvent.Code}","ConfirmEmail"));
                await _eventManager.ChangeStatusAsync(confirmationCodeEvent, EStatus.Completed);
                break;
            
            
            case ENotificationChannel.SMS:
                await _eventManager.ChangeStatusAsync(confirmationCodeEvent, EStatus.Processing);
                var result = _notificationApiClient.SendSms(confirmationCodeEvent.UserId, $"Code is {confirmationCodeEvent.Code}");
                if (!result.IsSuccessful)
                {
                    await _eventManager.ChangeStatusAsync(confirmationCodeEvent, EStatus.Failed);
                    await _eventManager.AddErrorMessageAsync(confirmationCodeEvent, result.ErrorMessage!);
                    return;
                }
                await _eventManager.ChangeStatusAsync(confirmationCodeEvent, EStatus.Completed);
                break;
            default:
                throw new InvalidOperationException("Invalid notification channel");
        }
    }
}