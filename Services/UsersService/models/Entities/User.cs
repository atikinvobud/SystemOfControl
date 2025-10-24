namespace BackEnd.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public required string HashPassword { get; set; }

    public UserInfo? UserInfoEntity { get; set; }
    public List<UserRole> UserRolesEntities { get; set; } = new List<UserRole>();
    public RefreshToken? RefreshTokenEntity { get; set; }
}