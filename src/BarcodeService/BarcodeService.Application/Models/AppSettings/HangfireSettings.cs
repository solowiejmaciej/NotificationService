namespace BarcodeService.Application.Models.AppSettings;

public class HangfireSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
    public static int RetryAttempts { get; set; } = 3;
}