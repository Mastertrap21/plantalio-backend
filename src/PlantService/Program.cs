using Core.Extension;
using Core.Handler;
using Core.ProgramBase;
using Core.Service;
using Microsoft.Extensions.DependencyInjection;
using PlantService.FunctionHandler;

namespace PlantService;

internal class Program : FunctionProgramBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<IFunctionHandler, GetPlantRequestHandler>();
        services.AddDatabase<PlantServiceContext>(ServiceMetadata);
    }

    private static void Main() => Boot<ServiceBuilder<Program>>();
}