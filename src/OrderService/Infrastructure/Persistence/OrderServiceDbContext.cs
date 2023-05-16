using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence;

public class OrderServiceDbContext : DbContext
{
    private readonly IDateTimeProvider _dateTime;

    public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options, IDateTimeProvider dateTime)
        : base(options)
    {
        _dateTime = dateTime;
    }

    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderItem> OrderItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderServiceDbContext).Assembly);

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