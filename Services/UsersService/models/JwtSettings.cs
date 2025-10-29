namespace BackEnd.Models;

public class JwtSettings
{
    public string SecretKey { get; set; } = null!;
    public int ExpireHours { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string CoockieName { get; set; } = null!;
}