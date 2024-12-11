namespace Pos.Configs;

public class AppSettings
{
    public string ServiceName { get; set; }
    public string Secret { get; set; }
    public int JwtTokenTTL { get; set; }
    public int RefreshTokenTTL { get; set; }
    public bool EnableDBLogging { get; set; }
}