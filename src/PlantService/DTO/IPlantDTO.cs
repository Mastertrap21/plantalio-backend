namespace PlantService.DTO;

public interface IPlantDTO
{
    Guid PlantId { get; set; }
    string? Name { get; set; }
}