namespace BackEnd.DTOs;

public record GetOrderProduct
{
    public int Amount { get; set; }
    public string Name { get; set; } = string.Empty;
}