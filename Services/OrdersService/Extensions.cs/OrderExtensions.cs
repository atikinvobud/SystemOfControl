using BackEnd.DTOs;
using BackEnd.Entities;

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

    public static Order ToEntity(this PostOrder postOrder, Guid userId)
    {
        return new Order()
        {
            Id = Guid.NewGuid(),
            StatusId = postOrder.StatusId,
            UserId = userId,
            DateOfCreation = DateTime.UtcNow,
            DateOfUpdate = DateTime.UtcNow,
        };
    }
}