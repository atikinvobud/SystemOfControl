namespace BackEnd.DTOs;

public record PutProduct
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Cost { get; set; }
}