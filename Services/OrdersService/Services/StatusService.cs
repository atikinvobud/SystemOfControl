using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Extensions;
using BackEnd.Repositories;
using BackEnd.Share;

namespace BackEnd.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository statusRepository;
    public StatusService(IStatusRepository statusRepository)
    {
        this.statusRepository = statusRepository;
    }

    public async Task<Status?> CheckStatus(string status)
    {
        List<Status> statuses = await statusRepository.GetAllStatuses();
        foreach (Status stat in statuses)
        {
            if (stat.Name.ToLower() == status.ToLower()) return stat;
        }
        return null;
    }

    public async  Task<Result<Guid>> CreateStatus(PostStatus postStatus)
    {
        Status status = postStatus.ToEntity();
        await statusRepository.CreateStatus(status);
        return Result<Guid>.Success(status.Id);
    }

    public async Task<Result<bool>> DeleteStatus(DeleteStatus deleteStatus)
    {
        Status? status = await statusRepository.GetStatusById(deleteStatus.Id);
        if (status is null) return Result<bool>.Error(ErrorCode.StatusNotFound);
        await statusRepository.DeleteStatus(status);
        return Result<bool>.Success(true);
    }

    public async Task<Result<List<GetStatus>>> GetAllStatuses()
    {
        List<Status> statuses = await statusRepository.GetAllStatuses();
        List<GetStatus> dtos = new List<GetStatus>();
        foreach (var status in statuses)
        {
            dtos.Add(status.ToDTO());
        }
        return Result<List<GetStatus>>.Success(dtos);
    }

    public async Task<Result<GetStatus>> GetStatusById(Guid Id)
    {
        Status? status = await statusRepository.GetStatusById(Id);
        if (status is null) return Result<GetStatus>.Error(ErrorCode.StatusNotFound);
        return Result<GetStatus>.Success(status.ToDTO());
    }

    public async Task<Result<bool>> UpdateStatus(PutStatus putStatus)
    {
        Status? status = await statusRepository.GetStatusById(putStatus.Id);
        if (status is null) return Result<bool>.Error(ErrorCode.StatusNotFound);
        status.Update(putStatus);
        await statusRepository.UpdateStatus(status);
        return Result<bool>.Success(true);
    }
}