namespace BackEnd.DTOs;

public record GetOrder
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public List<GetOrderProduct> Products { get; set; } = new List<GetOrderProduct>();
    public string Status { get; set; } = string.Empty;
    public double Total { get; set; }
    public DateTime TimeOfCreation { get; set; }
    public DateTime TimeOfUpdate { get; set; }
}