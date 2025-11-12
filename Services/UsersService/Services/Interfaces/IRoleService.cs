using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IRoleService
{
    Task<Result<GetRole>> GetById(Guid Id);
    Task<Result<List<GetRole>>> GetAll();
    Task<Result<Guid>> CreateRole(PostRole postRole);
    Task<Result<bool>> DeleteRole(DeleteRole deleteRole);
    Task<Result<bool>> UpdateRole(PutRole putRole);
}