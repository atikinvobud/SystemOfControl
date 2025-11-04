using BackEnd.Entities;

namespace BackEnd.Repositories;

public interface IStatusRepository
{
    Task<Status?> GetStatusById(Guid Id);
    Task<List<Status>> GetAllStatuses();
    Task<Guid> CreateStatus(Status status);
    Task DeleteStatus(Status status);
    Task UpdateStatus(Status status);
}