using BackEnd.Models.Entities;

namespace BackEnd.Repositories;

public interface IRefreshTokenRepository
{
    Task CreateToken(RefreshToken refreshToken);
    Task DeleteToken(RefreshToken refreshToken);
    Task<RefreshToken?> GetToken(Guid id);
}