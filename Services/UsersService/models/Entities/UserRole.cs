namespace BackEnd.Models.Entities;

public class UserRole
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid RoleId { get; set; }

    public Role? RoleEntity { get; set; }
    public User? UserEntity { get; set; }
}