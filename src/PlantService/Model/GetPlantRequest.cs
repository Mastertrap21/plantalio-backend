using Core.Model;

namespace PlantService.Model;

public class GetPlantRequest : FunctionPayload, IGetPlantRequest
{
    public Guid PlantId { get; set; }
}