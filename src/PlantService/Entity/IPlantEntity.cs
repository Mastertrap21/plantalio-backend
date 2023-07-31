namespace PlantService.Entity;

public interface IPlantEntity
{
    Guid PlantId { get; set; }
    string? Name { get; set; }
}