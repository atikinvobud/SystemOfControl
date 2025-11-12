namespace BackEnd.DTOs;

public record PutRole
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}