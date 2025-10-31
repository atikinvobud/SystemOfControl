using System.Security.Cryptography;
using BackEnd.Models.Entities;
using BackEnd.Repositories;

namespace BackEnd.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly HashService hashService;
    private readonly IRefreshTokenRepository repository;
    public RefreshTokenService(HashService hashService, IRefreshTokenRepository repository)
    {
        this.hashService = hashService;
        this.repository = repository;
    }
    public async Task<string> CreateToken(User user)
    {
        RefreshToken? entity = await repository.GetToken(user.Id);
        if (entity != null) await repository.DeleteToken(entity);
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);
        var hashToken = hashService.Hash(token);
        var refreshToken = new RefreshToken()
        {
            Id = user.Id,
            HashToken = hashToken,
            ExpiresAt = DateTime.UtcNow.AddDays(10)
        };
        await repository.CreateToken(refreshToken);
        return token;
    }

    public async Task<bool> IsTokenValid(string token, Guid id)
    {
        var RefreshToken = await repository.GetToken(id);
        if( RefreshToken is null) return false;
        if (!hashService.Verify(RefreshToken.HashToken, token)) return false;
        if (RefreshToken.ExpiresAt < DateTime.UtcNow) return false;
        return true;
    }
}