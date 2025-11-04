using BackEnd.Entities;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly Context context;
    public StatusRepository(Context context)
    {
        this.context = context;
    }

    private IQueryable<Status> GetInstances()
    {
        return context.Statuses.Include(s => s.OrdersEntity);
    }
    public async Task<Guid> CreateStatus(Status status)
    {
        await context.Statuses.AddAsync(status);
        await context.SaveChangesAsync();
        return status.Id;
    }

    public async Task DeleteStatus(Status status)
    {
        context.Statuses.Remove(status);
        await context.SaveChangesAsync();
    }

    public async Task<List<Status>> GetAllStatuses()
    {
        return await GetInstances().ToListAsync();
    }

    public async Task<Status?> GetStatusById(Guid Id)
    {
        return await GetInstances().FirstOrDefaultAsync(s => s.Id == Id);
    }

    public async Task UpdateStatus(Status status)
    {
        await context.SaveChangesAsync();
    }
}