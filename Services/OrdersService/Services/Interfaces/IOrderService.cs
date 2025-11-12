using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IOrderService
{
    Task<Result<List<GetOrder>>> GetOrders();
    Task<Result<GetOrder>> GetOrderById(Guid Id);
    Task<Result<List<GetOrder>>> GetOrdersWithPagination(Guid Id, Guid? StatusId, int Page, int PageSize);
    Task<Result<Guid>> Craeteorder(Guid Id);
    Task<Result<Guid>> AddProduct(PostOrderProduct postOrderProduct, Guid UserId);
    Task<Result<bool>> ChangeStatus(Guid orderId,string newStatus);
}