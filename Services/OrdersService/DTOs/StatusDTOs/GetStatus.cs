namespace BackEnd.DTOs;

public record GetStatus
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}