using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PlantService;

public class PlantServiceContextFactory : IPlantServiceContextFactory
{
    public PlantServiceContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PlantServiceContext>();
        var connectionString = "server=localhost;port=3307;user=user;password=password;database=PlantService";
        ((DbContextOptionsBuilder) optionsBuilder
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))).EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        return new PlantServiceContext(optionsBuilder.Options);
    }
}