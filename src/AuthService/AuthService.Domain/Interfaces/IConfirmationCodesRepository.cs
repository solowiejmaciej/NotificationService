#region

using AuthService.Domain.Entities;
using Shared.Enums;

#endregion

namespace AuthService.Domain.Interfaces;

public interface IConfirmationCodesRepository
{
    public Task<ConfirmationCode> GenerateCodeAsync(ENotificationChannel channel, string userId);
    public Task<bool> IsValidCode(string userId, string code, ENotificationChannel channel);
}