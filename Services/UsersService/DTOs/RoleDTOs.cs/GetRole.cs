namespace BackEnd.DTOs;

public record GetRole
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}