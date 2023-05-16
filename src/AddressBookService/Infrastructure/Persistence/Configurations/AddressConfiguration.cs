using AddressBookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBookService.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.DeliveryAddressId)
            .IsRequired();

        builder.HasMany(x => x.AddressItems)
            .WithOne(x => x.Addresses)
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}