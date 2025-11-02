using BackEnd.DTOs;
using BackEnd.Models;
using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace BackEnd.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly Context context;
    public RoleRepository(Context context)
    {
        this.context = context;
    }
    private IQueryable<Role> GetInstances()
    {
        return context.roleEntities.Include(r => r.UserRolesEntities).ThenInclude(ur => ur.UserEntity);
    }
    public async Task<List<Role>> GetAllRoles()
    {
        return await GetInstances().ToListAsync();
    }
    public async Task<Role?> GetEntityById(Guid id)
    {
        return await GetInstances().FirstOrDefaultAsync(r => r.Id ==id);
    }
    public async Task<Guid> CreateRole(Role role)
    {
        await context.roleEntities.AddAsync(role);
        await context.SaveChangesAsync();
        return role.Id;
    }
    public async Task UpdateRole(Role role)
    {
        context.roleEntities.Update(role);
        await context.SaveChangesAsync();
    }
    public async Task DeleteRole(Role role)
    {
        context.roleEntities.Remove(role);
        await context.SaveChangesAsync();
    }
}