using BackEnd.Models;
using BackEnd.Models.Entities;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly Context context;
    public RefreshTokenRepository(Context context)
    {
        this.context = context;
    }
    private IQueryable<RefreshToken> GetInstances()
    {
        return context.refreshTokensEntities.Include(rt => rt.UserEntity);
    }
    public async Task CreateToken(RefreshToken refreshToken)
    {
        await context.AddAsync(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetToken(Guid id)
    {
        return await GetInstances().FirstOrDefaultAsync(rt => rt.Id == id);
    }

    public async Task DeleteToken(RefreshToken refreshToken)
    {
        context.Remove(refreshToken);
        await context.SaveChangesAsync();

    }
}