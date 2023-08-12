using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Entity;

namespace UserService;

public class UserServiceContext : DbContext, IUserServiceContext
{
    protected EntityTypeBuilder<UserEntity>? EntityTypeBuilder;
    public DbSet<UserEntity>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserEntity>(entity =>
        {
            EntityTypeBuilder = entity;
            entity.Property(u => u.UserId).IsRequired();
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.Password).IsRequired();
            entity.HasKey(u => u.UserId);
            entity.HasIndex(u => u.Username).IsUnique();
        });
    }

    public UserServiceContext(DbContextOptions<UserServiceContext> options) : base(options)
    {
    }

    protected UserServiceContext()
    {
    }
}