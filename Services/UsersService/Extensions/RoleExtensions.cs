using BackEnd.DTOs;
using BackEnd.Models.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BackEnd.Extensions;

public static class RoleExtensions
{
    public static Role ToEntity(this PostRole postRole)
    {
        return new Role()
        {
            Id = Guid.NewGuid(),
            Name = postRole.Name
        };
    }
    public static GetRole ToDTO(this Role role)
    {
        return new GetRole()
        {
            Id = role.Id,
            Name = role.Name
        };
    }
    public static void Update(this Role role, PutRole putRole)
    {
        role.Name = putRole.Name ?? role.Name;
    }
}