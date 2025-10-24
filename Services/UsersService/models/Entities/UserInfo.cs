namespace BackEnd.Models.Entities;

public class UserInfo
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required DateTime TimeOfCreated { get; set; }
    public required DateTime TimeOfUpdate { get; set; }

    public User? UserEntity { get; set; }
}