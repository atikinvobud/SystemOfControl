using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace BackEnd.Services;

public class TokenAccessor : ITokenAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public TokenAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string? GetAccessToken()
    {
        var authHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
            return null;

        return authHeader.Substring("Bearer ".Length).Trim();
    }

    public Guid? GetUserId()
    {
        var token = GetAccessToken();
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdString = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userIdString, out Guid userId)) return userId;

        return null; 
    }


    public List<string> GetRoles()
    {
        var token = GetAccessToken();
        if (string.IsNullOrEmpty(token)) return new List<string>();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Claims
                       .Where(c => c.Type == ClaimTypes.Role)
                       .Select(c => c.Value)
                       .ToList();
    }
}
