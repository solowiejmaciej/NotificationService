#region

using Microsoft.AspNetCore.Identity;

#endregion

namespace AuthService.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? DeviceId { get; set; }
    public string Firstname { get; set; }
    public string Surname { get; set; }
}