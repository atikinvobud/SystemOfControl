namespace BackEnd.DTOs;

public record PostProduct
{
    public string Name { get; set; } = string.Empty;
    public double Cost { get; set; } 
}