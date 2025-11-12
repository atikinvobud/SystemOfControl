using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Extensions;
using BackEnd.Repositories;
using BackEnd.Share;

namespace BackEnd.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly IStatusService statusService;
    private readonly IProductService productService;
    public OrderService(IOrderRepository orderRepository, IStatusService statusService, IProductService productService)
    {
        this.orderRepository = orderRepository;
        this.statusService = statusService;
        this.productService = productService;
    }

    public async Task<Result<Guid>> AddProduct(PostOrderProduct postOrderProduct, Guid userId)
    {
        OrderProduct entity = postOrderProduct.ProductOrder();
        Order? order = await orderRepository.GetById(entity.OrderId);
        if (order is null) return Result<Guid>.Error(ErrorCode.OrderNotFound);
        if (order.UserId != userId) return Result<Guid>.Error(ErrorCode.ErorUser);
        bool flag = await productService.CheckProduct(entity.ProductId);
        if (!flag) return Result<Guid>.Error(ErrorCode.productNotFound);
        Guid id = await orderRepository.AddProduct(entity);

        return Result<Guid>.Success(id);
    }

    public async Task<Result<bool>> ChangeStatus(Guid orderId, string newStatus)
    {
        Order? order = await orderRepository.GetById(orderId);
        if (order == null) return Result<bool>.Error(ErrorCode.OrderNotFound);
        Status? stat = await statusService.CheckStatus(newStatus);
        if (stat == null) return Result<bool>.Error(ErrorCode.StatusNotFound);
        await orderRepository.UpdateStatus(order, stat.Id);
        return Result<bool>.Success(true);
    }

    public async Task<Result<Guid>> Craeteorder(Guid Id)
    {
        Order order = OrderExtensions.ToEntity(Id);
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