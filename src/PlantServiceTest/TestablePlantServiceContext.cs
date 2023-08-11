using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlantService;
using PlantService.Entity;

namespace PlantServiceTest;

public class TestablePlantServiceContext : PlantServiceContext
{
    public ModelBuilder ModelBuilder { get; private set; }
    public TestablePlantServiceContext(DbContextOptions<PlantServiceContext> options) : base(options)
    {
    }
    public TestablePlantServiceContext()
    {
    }
    public void TestOnModelCreating(ModelBuilder modelBuilder) => OnModelCreating(modelBuilder);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        base.OnModelCreating(modelBuilder);
    }

    public EntityTypeBuilder<PlantEntity> GetEntityTypeBuilder()
    {
        return EntityTypeBuilder;
    }
}