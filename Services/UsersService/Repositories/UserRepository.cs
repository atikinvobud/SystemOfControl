using BackEnd.Models;
using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Context context;

    public UserRepository(Context context)
    {
        this.context = context;
    }
    private IQueryable<User> GetInstances()
    {
        return context.userEntities.Include(u => u.UserInfoEntity).Include(u => u.RefreshTokenEntity).Include(u => u.UserRolesEntities).ThenInclude(ui => ui.RoleEntity);
    }
    
    public async Task<Guid?> CreateUser(User userEntity, UserInfo userInfoEntity)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            context.userEntities.Add(userEntity);
            context.userInfoEntities.Add(userInfoEntity);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            return userEntity.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }
    public Task<List<User>> GetAllUsers()
    {
        return GetInstances().AsNoTracking().ToListAsync();
    }

    public Task<User?> GetUserById(Guid  userId)
    {
        return GetInstances().AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    public Task<User?> GetUserByLogin(string login)
    {
        return GetInstances().FirstOrDefaultAsync(u => u.Login == login);
    }

    public Task Update()
    {
        return context.SaveChangesAsync();
    }

    public Task UpdateUser(User user, string newPassword)
    {
        user.HashPassword = newPassword;
        return context.SaveChangesAsync();
    }

    public async Task<Guid> AppointRole(UserRole userRole)
    {
        context.userRoleEntities.Add(userRole);
        await context.SaveChangesAsync();
        return userRole.Id;
    }

    public Task<List<User>> GelAllWithPagination(string? role, int page, int pageSize)
    {
        var query = context.userEntities.Include(u => u.UserInfoEntity).Include(u => u.UserRolesEntities).ThenInclude(ur => ur.RoleEntity).AsQueryable();
        if (role is not null) query = query.Where(r => r.UserRolesEntities.Any(ur => ur.RoleEntity!.Name == role));
        return query.OrderBy(u => u.Id).Skip((page -1)*pageSize).Take(pageSize).ToListAsync();
    }
}