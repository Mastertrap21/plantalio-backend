using Microsoft.EntityFrameworkCore.Design;

namespace UserService;

public interface IUserServiceContextFactory : IDesignTimeDbContextFactory<UserServiceContext>
{
}