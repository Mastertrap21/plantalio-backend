using Microsoft.EntityFrameworkCore;

namespace UserService;

public class UserServiceContextFactory : IUserServiceContextFactory
{
    private readonly DbContextOptionsBuilder<UserServiceContext> _optionsBuilder;

    public UserServiceContextFactory(DbContextOptionsBuilder<UserServiceContext> optionsBuilder)
    {
        _optionsBuilder = optionsBuilder;
    }

    public UserServiceContextFactory()
    {
        _optionsBuilder = new DbContextOptionsBuilder<UserServiceContext>();
    }

    public UserServiceContext CreateDbContext(string[] args)
    {
        const string connectionString = "server=localhost;port=3307;user=user;password=password;database=UserService";
        _optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        return new UserServiceContext(_optionsBuilder.Options);
    }
}