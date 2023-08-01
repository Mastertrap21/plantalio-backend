using Core.Handler;
using PlantService.Model;

namespace PlantService.FunctionHandler;

internal interface IGetPlantRequestHandler : IFunctionHandler
{
    void GetPlant(IGetPlantRequest request);
}