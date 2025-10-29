using BackEnd.Models.Entities;

namespace BackEnd.Services;

public interface IJwtService
{
    string GenerateToken(User user, List<string> roles);
    string GetCoockieName();
    int GetExpireHours();
}