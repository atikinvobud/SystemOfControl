namespace BackEnd.DTOs;

public record PutUserInfo
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}