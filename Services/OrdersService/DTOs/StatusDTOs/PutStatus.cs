namespace BackEnd.DTOs;

public record PutStatus
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}