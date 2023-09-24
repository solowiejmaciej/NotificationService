using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace AuthService.Infrastructure.Repositories;

public class ConfirmationCodesRepository : IConfirmationCodesRepository
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<ConfirmationCodesRepository> _logger;

    public ConfirmationCodesRepository(
        ICacheService cacheService,
        ILogger<ConfirmationCodesRepository> logger
        )
    {
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async Task<ConfirmationCode> GenerateCodeAsync(ENotificationChannel channel, string userId)
    {
         await _cacheService.RemoveDataAsync($"code:{channel}:{userId}");
         var code = new ConfirmationCode()
         {
            NotificationChannel = channel,
         };
         await _cacheService.SetDataAsync($"code:{channel}:{userId}", code, code.ExpiresAt);
         return code;
        
    }

    public async Task<bool> IsValidCode(string userId, string code, ENotificationChannel channel)
    {
        var cachedCode = await _cacheService.GetDataAsync<ConfirmationCode>($"code:{channel}:{userId}");
        if (cachedCode == null)
        {
            _logger.LogInformation("Code not found");
            return false;
        }
        if (cachedCode.Code.ToString() != code)
        {
            _logger.LogInformation("Code is invalid");
            return false;
        }
        return true;
    }
}