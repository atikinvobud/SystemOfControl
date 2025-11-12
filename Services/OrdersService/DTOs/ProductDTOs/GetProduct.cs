namespace BackEnd.DTOs;

public record GetProduct
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Cost { get; set; }
}