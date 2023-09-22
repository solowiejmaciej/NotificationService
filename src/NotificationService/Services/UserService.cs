using System.Net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationService.Models.AppSettings;
using NotificationService.Models.Dtos;
using RestSharp;
using Shared.Exceptions;

namespace NotificationService.Services;

public interface IUserService
{
    public Task<UserDto> GetUser(string userId);
}
public class UserService : IUserService
{
    private readonly ILogger<IUserService> _logger;
    private readonly AuthApiConfig _config;

    public UserService(
        IOptions<AuthApiConfig> config, 
        ILogger<IUserService> logger)
    {
        _config = config.Value;
        _logger = logger;
    }
    
    public async Task<UserDto> GetUser(string userId)
    {
        var baseUrl = _config.ApiUrl;
 
        var options = new RestClientOptions(baseUrl)
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest($"/api/User/{userId}", Method.Get);
        request.AddHeader("x-api-key", _config.ApiKey);
        request.AddHeader("Content-Type", "application/json");

        var response = await client.ExecuteAsync<UserDto>(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation(response.ErrorMessage);
            throw new NotFoundException($"user: {userId} not found");
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(response.ErrorMessage);
            throw new RequestFailedException($"Unable to authorize to AuthService");
        }
        
        if (response.IsSuccessStatusCode)
        {
            var userDto = JsonConvert.DeserializeObject<UserDto>(response.Content);
            return userDto;
        }
        
        _logger.LogError(response.ErrorMessage);
        throw new RequestFailedException($"Request to AuthService for user: {userId} failed");
    }
}
