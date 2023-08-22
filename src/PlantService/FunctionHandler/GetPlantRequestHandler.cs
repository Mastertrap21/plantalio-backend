using Core.Messaging;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlantService.DTO;
using PlantService.Model;

namespace PlantService.FunctionHandler;

internal class GetPlantRequestHandler : Core.Handler.FunctionHandler, IGetPlantRequestHandler
{
    private readonly IProducer _producer;
    private readonly IDbContextFactory<PlantServiceContext> _contextFactory;

    public GetPlantRequestHandler(ILogger logger, IProducer producer, IDbContextFactory<PlantServiceContext> contextFactory) : base(logger)
    {
        _producer = producer;
        _contextFactory = contextFactory;
    }

    public override void RegisterFuncListeners(IFunctionService? service)
    {
        service?.Register<GetPlantRequest>(GetPlant);
    }

    public void GetPlant(IGetPlantRequest request)
    {
        var plantId = request.PlantId;
        var response = new GetPlantResponse();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            
            Log.LogInformation(LoggingMessageTemplates.GetPlantRequestHandlePlantIdCheckInfo, plantId);
            if (context.Plants != null)
            {
                var plant = context.Plants
                    .SingleOrDefault(p => p.PlantId == request.PlantId);
                if (plant != null)
                {
                    Log.LogInformation(LoggingMessageTemplates.GetPlantRequestHandlePlantIdFoundInfo, plantId);
                    response.Plant = new PlantDto
                    {
                        PlantId = plant.PlantId,
                        Name = plant.Name
                    };
                }
                else
                {
                    Log.LogInformation(LoggingMessageTemplates.GetPlantRequestHandlePlantIdNotFoundInfo, plantId);
                }
            }
        }
        catch (Exception e)
        {
            Log.LogError(e, LoggingMessageTemplates.GetPlantRequestHandleFailed, request);
        }

        _producer.Respond(request, response);
    }
}