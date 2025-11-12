using BackEnd.Models.Entities;

namespace BackEnd.Services;

public interface IRefreshTokenService
{
    Task<bool> IsTokenValid(string token, Guid id);
    Task<string> CreateToken(User user);
    Task<bool> DeleteToken(string token, Guid id);
}