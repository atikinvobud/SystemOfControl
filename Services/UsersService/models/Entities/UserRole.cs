namespace BackEnd.Models.Entities;

public class UserRole
{
    public Guid Id { get; set; }
    public required int UserId { get; set; }
    public required int RoleId { get; set; }

    public Role? RoleEntity { get; set; }
    public User? UserEntity { get; set; }
}