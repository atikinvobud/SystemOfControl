namespace BackEnd.Models.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public required string HashToken { get; set; }
    public required DateTime ExpiresAt { get; set; }

    public User? UserEntity { get; set; }
}