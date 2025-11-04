namespace BackEnd.Entities;

public class OrderProduct
{
    public Guid Id { get; set; }
    public required Guid OrderId { get; set; }
    public required Guid ProductId { get; set; }
    public required int Amount { get; set; } 

    public Order? OrderEntity { get; set; }
    public Product? ProductEntity { get; set; }
}