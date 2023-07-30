using Microsoft.EntityFrameworkCore;
using UserService.Entity;

namespace UserService;

public interface IUserServiceContext
{
    DbSet<UserEntity>? Users { get; set; }
}