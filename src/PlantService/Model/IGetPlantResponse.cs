using PlantService.DTO;

namespace PlantService.Model;

public interface IGetPlantResponse
{
    PlantDto? Plant { get; set; }
}