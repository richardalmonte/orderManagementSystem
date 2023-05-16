using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AddressBookService.Domain.Entities;

namespace AddressBookService.Infrastructure.Persistence.Configurations;

public class AddressItemConfiguration : IEntityTypeConfiguration<AddressItem>
{
    public void Configure(EntityTypeBuilder<AddressItem> builder)
    {
        builder.ToTable("AddressItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AddressId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();
    }
}