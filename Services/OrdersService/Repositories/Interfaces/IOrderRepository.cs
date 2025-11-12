using BackEnd.Entities;

namespace BackEnd.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetById(Guid Id);
    Task<List<Order>> GetAll();
    Task<List<Order>> GetWithPagination(Guid UserId, Guid? StatusId, int page, int pageSize);
    Task<Guid> AddProduct(OrderProduct orderProduct);
    Task<Guid> CreateOrder(Order order);
}