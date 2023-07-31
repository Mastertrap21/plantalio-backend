using Microsoft.EntityFrameworkCore;
using PlantService.Entity;

namespace PlantService;

public interface IPlantServiceContext
{
    DbSet<PlantEntity>? Plants { get; set; }
}