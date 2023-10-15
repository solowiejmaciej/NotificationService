namespace BarcodeService.Application.Hangfire;

public static class HangfireRetryAttempts
{
    public const int DEFAULT = 3;
    public const int NONE = 0;
    public const int HIGH = 10;
}