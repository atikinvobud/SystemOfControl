using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IUserService
{
    Task<Result<GetUser>> GetUser(Guid Id);
    Task<Result<bool>> UpdateUserInfo(PutUserInfo putUserInfo);
    Task<Result<Guid>> AppointRole(PostUserRole postUserRole);
    Task<Result<List<GetUser>>> GetUsersWithPagination(string? role, int page, int pageSize);
}