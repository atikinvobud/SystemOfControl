using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Models.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);

        builder.HasOne(ur => ur.RoleEntity)
            .WithMany(u => u.UserRolesEntities)
            .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.RoleEntity)
            .WithMany(r => r.UserRolesEntities)
            .HasForeignKey(ur => ur.RoleId);
    }
}