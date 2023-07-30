using Microsoft.EntityFrameworkCore;
using UserService.Entity;

namespace UserService;

public class UserServiceContext : DbContext
{
    public DbSet<UserEntity>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserEntity>(entity =>
        {
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