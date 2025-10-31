using BackEnd.Models.Entities;

namespace BackEnd.Repositories;

public interface IUserRepository
{
    Task<Guid?> CreateUser(User userEntity, UserInfo userInfoEntity);
    Task<User?> GetUserById(Guid userId);
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserByLogin(string login);
    Task UpdateUser(User user, string newPassword);
    Task Update();
}