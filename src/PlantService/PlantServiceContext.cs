using Microsoft.EntityFrameworkCore;
using PlantService.Entity;

namespace PlantService;

public class PlantServiceContext : DbContext, IPlantServiceContext
{
    public DbSet<PlantEntity>? Plants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PlantEntity>(entity =>
        {
            entity.Property(p => p.PlantId).IsRequired();
            entity.Property(p => p.Name).IsRequired();
            entity.HasKey(p => p.PlantId);
        });
    }

    public PlantServiceContext(DbContextOptions<PlantServiceContext> options) : base(options)
    {
    }

    protected PlantServiceContext()
    {
    }
}