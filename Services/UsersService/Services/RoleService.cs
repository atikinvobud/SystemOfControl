using BackEnd.DTOs;
using BackEnd.Extensions;
using BackEnd.Models.Entities;
using BackEnd.Repositories;
using BackEnd.Share;

namespace BackEnd.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }
    public async Task<Result<Guid>> CreateRole(PostRole postRole)
    {
        Role role = postRole.ToEntity();
        Guid id = await roleRepository.CreateRole(role);
        return Result<Guid>.Success(id);
    }

    public async Task<Result<bool>> DeleteRole(DeleteRole deleteRole)
    {
        Role? role = await roleRepository.GetEntityById(deleteRole.Id);
        if (role is null) return Result<bool>.Error(ErrorCode.RoleNotFound);
        await roleRepository.DeleteRole(role);
        return Result<bool>.Success(true);
    }

    public async Task<Result<List<GetRole>>> GetAll()
    {
        List<Role> roles = await roleRepository.GetAllRoles();
        List<GetRole> dtos = new List<GetRole>();
        foreach(var role in roles)
        {
            dtos.Add(role.ToDTO());
        }
        return Result<List<GetRole>>.Success(dtos);
    }

    public async Task<Result<GetRole>> GetById(Guid Id)
    {
        Role? role = await roleRepository.GetEntityById(Id);
        if (role is null) return Result<GetRole>.Error(ErrorCode.RoleNotFound);
        return Result<GetRole>.Success(role.ToDTO());
    }

    public async  Task<Result<bool>> UpdateRole(PutRole putRole)
    {
        Role? role = await roleRepository.GetEntityById(putRole.Id);
        if (role is null) return Result<bool>.Error(ErrorCode.RoleNotFound);
        role.Update(putRole);
        await roleRepository.UpdateRole(role);
        return Result<bool>.Success(true);
    }
}