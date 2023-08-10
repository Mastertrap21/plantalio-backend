using Microsoft.EntityFrameworkCore;
using PlantService;

namespace PlantServiceTest;

public class TestablePlantServiceContext : PlantServiceContext
{
    public ModelBuilder ModelBuilder { get; private set; }
    public TestablePlantServiceContext(DbContextOptions<PlantServiceContext> options) : base(options)
    {
    }
    public void TestOnModelCreating(ModelBuilder modelBuilder) => OnModelCreating(modelBuilder);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        base.OnModelCreating(modelBuilder);
    }
}