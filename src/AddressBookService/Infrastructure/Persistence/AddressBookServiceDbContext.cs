using Microsoft.EntityFrameworkCore;
using AddressBookService.Application.Interfaces;
using AddressBookService.Domain.Entities;

namespace AddressBookService.Infrastructure.Persistence;

public class AddressBookServiceDbContext : DbContext
{
    private readonly IDateTimeProvider _dateTime;

    public AddressBookServiceDbContext(DbContextOptions<AddressBookServiceDbContext> options,
        IDateTimeProvider dateTime)
        : base(options)
    {
        _dateTime = dateTime;
    }

    public DbSet<Address> Addresses { get; set; } = default!;
    public DbSet<AddressItem> AddressItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddressBookServiceDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        var currentDateTime = _dateTime.UtcNow;

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = currentDateTime;
            }

            ((BaseEntity)entity.Entity).UpdatedAt = currentDateTime;
        }
    }
}