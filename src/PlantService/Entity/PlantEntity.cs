namespace PlantService.Entity;

public class PlantEntity : IPlantEntity
{
    public Guid PlantId { get; set; }
    public string? Name { get; set; }
}