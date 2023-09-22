namespace AuthService.Application.Models.AppSettings
{
    public class JwtSettings
    {
        public string PublicKey { get; set; }
        public int ExpireMinutes { get; set; }
        public string Issuer { get; set; }
        public string PrivateKey { get; set; }
    }
}