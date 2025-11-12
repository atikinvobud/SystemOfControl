namespace BackEnd.DTOs;

public record PostUserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}