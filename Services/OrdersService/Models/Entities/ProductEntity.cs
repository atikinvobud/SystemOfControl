namespace BackEnd.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required double Cost { get; set; }

    public List<OrderProduct> OrderProductsEntity{ get; set; } = new List<OrderProduct>();
}