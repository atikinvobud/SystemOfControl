using BackEnd.Models;
using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories;

public class UserInfoRepository : IUserInfoRepository
{
    private readonly Context context;
    public UserInfoRepository(Context context)
    {
        this.context = context;
    }

    private IQueryable<UserInfo> GetInstances()
    {
        return context.userInfoEntities.Include(ui => ui.UserEntity);
    }
    public Task<UserInfo?> GetUserInfo(Guid Id)
    {
        return GetInstances().FirstOrDefaultAsync( ui => ui.Id == Id);
    }

    public Task UpdateUserInfo(UserInfo UserInfo)
    {
        return context.SaveChangesAsync();
    }
}