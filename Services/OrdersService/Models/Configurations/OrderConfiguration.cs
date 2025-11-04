using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasMany(o => o.OrderProductsEntities)
            .WithOne(op => op.OrderEntity);

        builder.HasOne(o => o.StatusEntity)
            .WithMany(s => s.OrdersEntity)
            .HasForeignKey(o => o.StatusId);

    }
}