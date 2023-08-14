using Microsoft.EntityFrameworkCore;

namespace PlantService;

public class PlantServiceContextFactory : IPlantServiceContextFactory
{
    private readonly DbContextOptionsBuilder<PlantServiceContext> _optionsBuilder;

    public PlantServiceContextFactory(DbContextOptionsBuilder<PlantServiceContext> optionsBuilder)
    {
        _optionsBuilder = optionsBuilder;
    }

    public PlantServiceContextFactory()
    {
        _optionsBuilder = new DbContextOptionsBuilder<PlantServiceContext>();
    }
    public PlantServiceContext CreateDbContext(string[] args)
    {
        const string connectionString = "server=localhost;port=3307;user=user;password=password;database=PlantService";
        _optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        return new PlantServiceContext(_optionsBuilder.Options);
    }
}