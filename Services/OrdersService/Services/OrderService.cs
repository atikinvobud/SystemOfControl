using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Extensions;
using BackEnd.Repositories;
using BackEnd.Share;

namespace BackEnd.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }

    public Task<Result<Guid>> AddProduct(PostOrderProduct postOrderProduct)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Guid>> Craeteorder(Guid Id, PostOrder postOrder)
    {
        Order order = postOrder.ToEntity(Id);
        await orderRepository.CreateOrder(order);
        return Result<Guid>.Success(order.Id);
    }

    public async Task<Result<GetOrder>> GetOrderById(Guid Id)
    {
        Order? order = await orderRepository.GetById(Id);
        if (order is null) return Result<GetOrder>.Error(ErrorCode.OrderNotFound);
        return Result<GetOrder>.Success(order!.ToDTO());
    }

    public async Task<Result<List<GetOrder>>> GetOrders()
    {
        List<Order> orders = await orderRepository.GetAll();
        List<GetOrder> dtos = new List<GetOrder>();
        foreach (var order in orders)
        {
            dtos.Add(order.ToDTO());
        }
        return Result<List<GetOrder>>.Success(dtos);
    }

    public async Task<Result<List<GetOrder>>> GetOrdersWithPagination(Guid Id, Guid? StatusId, int Page, int PageSize)
    {
        List<Order> orders = await orderRepository.GetWithPagination(Id, StatusId, Page, PageSize);
        List<GetOrder> dtos = new List<GetOrder>();
        foreach (var order in orders)
        {
            dtos.Add(order.ToDTO());
        }
        return Result<List<GetOrder>>.Success(dtos);
    }
}