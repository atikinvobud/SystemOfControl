using BackEnd.DTOs;
using BackEnd.Models.Entities;

namespace BackEnd.Extensions;

public static class AuthorizationExtension
{
    public static User ToEntity(this RegistrDTO dto)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            Login = dto.Login,
            HashPassword = dto.Password
        };
    }
    public static UserInfo ToUserInfo(this RegistrDTO dto, Guid userId)
    {
        return new UserInfo()
        {
            Id = userId,
            Name = dto.Name,
            Surname = dto.Surname,
            TimeOfCreated = DateTime.UtcNow,
            TimeOfUpdate = DateTime.UtcNow
        };
    }

    public static List<string> GetRoles(this User user)
    {
        var roles = user.UserRolesEntities?.Select(ur => ur.RoleEntity?.Name)
            .Where(r => !string.IsNullOrEmpty(r))
            .Select(r => r!).ToList() ?? new List<string>();
        return roles;
    }

    public static T With<T>(this T obj, Action<T> setter)
    {
        setter(obj);
        return obj;
    }
}