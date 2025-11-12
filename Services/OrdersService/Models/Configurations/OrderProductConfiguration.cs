using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(op => op.Id);

        builder.HasOne(op => op.ProductEntity)
            .WithMany(p => p.OrderProductsEntity)
            .HasForeignKey(op => op.ProductId);

        builder.HasOne(op => op.OrderEntity)
            .WithMany(o => o.OrderProductsEntities)
            .HasForeignKey(op => op.OrderId);
    }
}