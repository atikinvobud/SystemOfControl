using BackEnd.DTOs;
using BackEnd.Models.Entities;

namespace BackEnd.Extensions;

public static class UserExtensions
{
    public static GetUser ToDTO(this User user)
    {
        return new GetUser()
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.UserInfoEntity!.Name,
            SurName = user.UserInfoEntity!.Surname
        };
    }
    public static void UpdateUserInfo(this UserInfo userInfo, PutUserInfo putUserInfo)
    {
        userInfo.Name = putUserInfo.Name ?? userInfo.Name;
        userInfo.Surname = putUserInfo.Surname ?? userInfo.Surname;
        userInfo.TimeOfUpdate = DateTime.UtcNow;
    }

    public static UserRole ToEntity(this PostUserRole userRole)
    {
        return new UserRole()
        {
            Id = Guid.NewGuid(),
            UserId = userRole.UserId,
            RoleId = userRole.RoleId
        };
    }
}