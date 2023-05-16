using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();
    }
}