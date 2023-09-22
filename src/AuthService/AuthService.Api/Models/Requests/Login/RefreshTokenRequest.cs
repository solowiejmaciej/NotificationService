namespace AuthService.Api.Models.Requests.Login;

public class RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}