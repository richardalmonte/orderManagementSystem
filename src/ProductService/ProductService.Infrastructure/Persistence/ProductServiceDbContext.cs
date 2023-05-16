using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using BaseEntity = UserService.Domain.Entities.BaseEntity;

namespace ProductService.Infrastructure.Persistence;

public class ProductServiceDbContext : DbContext
{
    public readonly IDateTimeProvider _dateTime;

    public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options, IDateTimeProvider dateTime)
        : base(options)
    {
        _dateTime = dateTime;
    }

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductServiceDbContext).Assembly);

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