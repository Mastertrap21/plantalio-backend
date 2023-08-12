using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService;
using UserService.Entity;

namespace UserServiceTest;

public class TestableUserServiceContext : UserServiceContext
{
    public ModelBuilder ModelBuilder { get; private set; }
    public TestableUserServiceContext(DbContextOptions<UserServiceContext> options) : base(options)
    {
    }
    public TestableUserServiceContext()
    {
    }
    public void TestOnModelCreating(ModelBuilder modelBuilder) => OnModelCreating(modelBuilder);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        base.OnModelCreating(modelBuilder);
    }

    public EntityTypeBuilder<UserEntity> GetEntityTypeBuilder()
    {
        return EntityTypeBuilder;
    }
}