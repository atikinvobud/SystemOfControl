using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Models.Configurations;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasKey(ui => ui.Id);

        builder.HasOne(ui => ui.UserEntity)
            .WithOne(u => u.UserInfoEntity)
            .HasForeignKey<UserInfo>(ui => ui.Id);
    }
}