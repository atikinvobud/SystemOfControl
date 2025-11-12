using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Share;

namespace BackEnd.Extensions;

public static class OrderExtensions
{
    public static GetOrder ToDTO(this Order order)
    {
        return new GetOrder()
        {
            OrderId = order.Id,
            UserId = order.UserId,
            Status = order.StatusEntity!.Name,
            TimeOfCreation = order.DateOfCreation,
            TimeOfUpdate = order.DateOfUpdate,
            Products = order.OrderProductsEntities.Select(op => new GetOrderProduct()
            {
                Amount = op.Amount,
                Name = op.ProductEntity!.Name,
            }).ToList(),
            Total = order.OrderProductsEntities.Sum(op => op.Amount * op.ProductEntity!.Cost)
        };
    }

    public static Order ToEntity( Guid userId)
    {
        return new Order()
        {
            Id = Guid.NewGuid(),
            StatusId = OrderCodeService.GetMessage(OrderStatuses.Created),
            UserId = userId,
            DateOfCreation = DateTime.UtcNow,
            DateOfUpdate = DateTime.UtcNow,
        };
    }

    public static OrderProduct ProductOrder(this PostOrderProduct postOrderProduct)
    {
        return new OrderProduct()
        {
            ProductId = postOrderProduct.ProductId,
            OrderId = postOrderProduct.OrderId,
            Amount = postOrderProduct.Amount
        };
    }
}