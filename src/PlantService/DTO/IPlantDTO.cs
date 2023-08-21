namespace PlantService.DTO;

public interface IPlantDto
{
    Guid PlantId { get; set; }
    string? Name { get; set; }
}