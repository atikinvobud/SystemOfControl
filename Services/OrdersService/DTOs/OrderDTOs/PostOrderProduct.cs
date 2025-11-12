namespace BackEnd.DTOs;

public record PostOrderProduct
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Amount { get; set; }
}