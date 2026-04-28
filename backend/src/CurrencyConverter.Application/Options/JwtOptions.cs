namespace CurrencyConverter.Application.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "CurrencyConverter";
    public string Audience { get; set; } = "CurrencyConverter.Clients";
    public string SigningKey { get; set; } = "super-secret-signing-key-at-least-32chars";
    public int ExpiryMinutes { get; set; } = 60;
}
