using BackEnd.Entities;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly Context context;
    public OrderRepository(Context context)
    {
        this.context = context;
    }
    private IQueryable<Order> GetInstances()
    {
        return context.Orders.Include(o => o.StatusEntity).Include(o => o.OrderProductsEntities).ThenInclude(op => op.ProductEntity);
    }
    public Task<Order?> GetById(Guid Id)
    {
        return GetInstances().FirstOrDefaultAsync(o => o.Id == Id);
    }

    public Task<List<Order>> GetWithPagination(Guid UserId,Guid? StatusId, int page, int pageSize)
    {
        var query = GetInstances().Where(o => o.UserId == UserId);
        if(StatusId is not null) query = query.Where(o => o.StatusId == StatusId);
        return query.OrderBy(o => o.Id).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
    }

    public Task<List<Order>> GetAll()
    {
        return GetInstances().ToListAsync();
    }

    public async Task<Guid> CreateOrder(Order order)
    {
        await context.AddAsync(order);
        await context.SaveChangesAsync();
        return order.Id;
    }

    public async Task<Guid> AddProduct(OrderProduct orderProduct)
    {
        await context.OrderProducts.AddAsync(orderProduct);
        await context.SaveChangesAsync();
        return orderProduct.Id;
    }

    public Task UpdateStatus(Order order, Guid statusId)
    {
        order.StatusId = statusId;
        return context.SaveChangesAsync();
    }
}