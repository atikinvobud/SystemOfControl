using BackEnd.Services;
using Microsoft.AspNetCore.Http;

public class CoockieService : ICoockieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CoockieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool SetCookie(string name, string token, int days)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        if (response == null) return false;

        response.Cookies.Append(name, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(days)
        });
        return true;
    }

    public string? GetCookie(string key)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null) return null;
        request.Cookies.TryGetValue(key, out string? value);
        return value;
    }

    public void DeleteCookie(string key)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        response?.Cookies.Delete(key);
    }
}
