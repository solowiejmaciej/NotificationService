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
    public void SendEmail(string userId, SendEmailBody body);
    public Task SendPush(string userId);
    public void SendSms(string userId, string body);
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
    

    public void SendEmail(string userId, SendEmailBody body)
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

    public void SendSms(string userId, string body)
    {
        var baseUrl = _config.ApiUrl;
         
        var options = new RestClientOptions(baseUrl)
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest($"/api/Smses?UserId={userId}", Method.Post);
        //request.AddHeader("x-api-key", _config.ApiKey);
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new
        {
            Content = body
        });
        var response = client.Execute(request);

        _logger.LogInformation($"Request fired to {baseUrl}");
        _logger.LogInformation(response.StatusCode.ToString());
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation(response.ErrorMessage);
            throw new RequestFailedException($"sms to user {userId} failed");
        }
         
        _logger.LogInformation($"sms sent");
    }
}
