using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlantService.Entity;

namespace PlantService;

public class PlantServiceContext : DbContext, IPlantServiceContext
{
    protected EntityTypeBuilder<PlantEntity>? EntityTypeBuilder;
    public DbSet<PlantEntity>? Plants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PlantEntity>(entity =>
        {
            EntityTypeBuilder = entity;
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