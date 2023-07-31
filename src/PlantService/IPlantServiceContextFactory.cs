using Microsoft.EntityFrameworkCore.Design;

namespace PlantService;

public interface IPlantServiceContextFactory : IDesignTimeDbContextFactory<PlantServiceContext>
{
}