namespace BackEnd.Entities;

public class Status
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public List<Order> OrdersEntity{ get; set; } = new List<Order>();
}