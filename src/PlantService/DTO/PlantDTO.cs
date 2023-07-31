namespace PlantService.DTO;

public class PlantDTO : IPlantDTO
{
    public Guid PlantId { get; set; }
    public string? Name { get; set; }
}