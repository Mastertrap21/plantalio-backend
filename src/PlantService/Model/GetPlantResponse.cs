using Core.Model;
using PlantService.DTO;

namespace PlantService.Model;

public class GetPlantResponse : FunctionPayload, IGetPlantResponse
{
    public PlantDto? Plant { get; set; }
}