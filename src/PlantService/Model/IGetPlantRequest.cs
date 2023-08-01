using Core.Model;

namespace PlantService.Model;

public interface IGetPlantRequest : IFunctionPayload
{
    Guid PlantId { get; set; }
}