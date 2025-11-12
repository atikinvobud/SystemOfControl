using BackEnd.Models.Entities;

namespace BackEnd.Repositories;

public interface IUserInfoRepository
{
    Task<UserInfo?> GetUserInfo(Guid Id);
    Task UpdateUserInfo(UserInfo UserInfo);
}