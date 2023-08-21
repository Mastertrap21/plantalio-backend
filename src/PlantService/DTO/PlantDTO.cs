namespace PlantService.DTO;

public class PlantDto : IPlantDto
{
    public Guid PlantId { get; set; }
    public string? Name { get; set; }
}