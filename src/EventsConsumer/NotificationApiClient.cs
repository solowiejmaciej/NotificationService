using System;
using System.Threading.Tasks;
using EventsConsumer.Models.AppSettings;
using EventsConsumer.Models.Body;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using Shared.Exceptions;

namespace EventsConsumer;


public interface INotificationApiClient
{
    public void SendEmail(string userId);
    public Task SendPush(string userId);
    public Task SendSms(string userId);
}

public class NotificationApiClient : INotificationApiClient
{
    private readonly ILogger<NotificationApiClient> _logger;
    private readonly NotificationApiSettings _config;
    
    public NotificationApiClient(
        ILogger<NotificationApiClient> logger, 
        IOptions<NotificationApiSettings> config
        )
    {
        _logger = logger;
        _config = config.Value;
    }
    

    public void SendEmail(string userId)
    {
         var baseUrl = _config.ApiUrl;
         
         var options = new RestClientOptions(baseUrl)
         {
             MaxTimeout = -1,
         };
         var client = new RestClient(options);
         var request = new RestRequest($"/api/Emails?UserId={userId}", Method.Post);
         request.AddHeader("x-api-key", _config.ApiKey);
         request.AddHeader("Content-Type", "application/json");
         var body = new SendEmailBody($"Cześć dziękujemy za rejestrację!", "Hello there!");
         request.AddJsonBody(body);
         var response = client.Execute(request);

         _logger.LogInformation($"Request fired to {baseUrl}");
 
         if (!response.IsSuccessStatusCode)
         {
             _logger.LogInformation(response.ErrorMessage);
             throw new RequestFailedException($"email to user {userId} failed");
         }
         
         _logger.LogInformation($"Email sent");
    }

    public Task SendPush(string userId)
    {
        throw new NotImplementedException();
    }

    public Task SendSms(string userId)
    {
        throw new NotImplementedException();
    }
}
