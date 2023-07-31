using PlantService.DTO;

namespace PlantService.Model;

public interface IGetPlantResponse
{
    PlantDTO? Plant { get; set; }
}