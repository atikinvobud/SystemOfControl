using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasOne(u => u.UserInfoEntity)
            .WithOne(ui => ui.UserEntity);

        builder.HasOne(u => u.RefreshTokenEntity)
            .WithOne(rt => rt.UserEntity);

        builder.HasMany(u => u.UserRolesEntities)
            .WithOne(ur => ur.UserEntity);
    }
}