using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IStatusService
{
    Task<Result<GetStatus>> GetStatusById(Guid Id);
    Task<Result<List<GetStatus>>> GetAllStatuses();
    Task<Result<Guid>> CreateStatus(PostStatus postStatus);
    Task<Result<bool>> UpdateStatus(PutStatus putStatus);
    Task<Result<bool>> DeleteStatus(DeleteStatus deleteStatus);
    Task<Status?> CheckStatus(string status);
}