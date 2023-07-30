using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService;

public class UserServiceContextFactory : IUserServiceContextFactory
{
    public UserServiceContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserServiceContext>();
        var connectionString = "server=localhost;port=3307;user=user;password=password;database=UserService";
        ((DbContextOptionsBuilder) optionsBuilder
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))).EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        return new UserServiceContext(optionsBuilder.Options);
    }
}