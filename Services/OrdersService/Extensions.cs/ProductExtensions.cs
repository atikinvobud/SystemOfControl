using BackEnd.DTOs;
using BackEnd.Entities;

namespace BackEnd.Extensions;

public static class ProductExtensions
{
    public static Product ToEntity(this PostProduct postProduct)
    {
        return new Product()
        {
            Id = Guid.NewGuid(),
            Name = postProduct.Name,
            Cost = postProduct.Cost
        };
    }

    public static GetProduct ToDTO(this Product product)
    {
        return new GetProduct()
        {
            Id = product.Id,
            Name = product.Name,
            Cost = product.Cost
        };
    }

    public static void Update(this Product product, PutProduct putProduct)
    {
        product.Name = putProduct.Name ?? product.Name;
        product.Cost = putProduct.Cost ?? product.Cost;
    }
}