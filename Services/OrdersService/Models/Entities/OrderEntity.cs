namespace BackEnd.Entities;

public class Order
{
    public Guid Id { get; set; }
    public required Guid StatusId { get; set; }
    public required Guid UserId { get; set; }
    public required DateTime DateOfCreation { get; set; }
    public required DateTime DateOfUpdate { get; set; }

    public Status? StatusEntity { get; set; }
    public List<OrderProduct> OrderProductsEntities { get; set; } = new List<OrderProduct>();
    
}