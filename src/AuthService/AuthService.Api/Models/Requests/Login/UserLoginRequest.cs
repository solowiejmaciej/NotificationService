namespace AuthService.Api.Models.Requests.Login;

public class UserLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}