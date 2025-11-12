using BackEnd.Models.Entities;

namespace BackEnd.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> GetAllRoles();
    Task<Role?> GetEntityById(Guid id);
    Task<Guid> CreateRole(Role role);
    Task UpdateRole(Role role);
    Task DeleteRole(Role role);
}