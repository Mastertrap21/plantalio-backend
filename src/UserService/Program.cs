using Core.Extension;
using Core.Handler;
using Core.ProgramBase;
using Core.Service;
using Microsoft.Extensions.DependencyInjection;
using UserService.FunctionHandler;

namespace UserService;

internal class Program : FunctionProgramBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<IFunctionHandler, LoginRequestHandler>();
        services.AddSingleton<IFunctionHandler, RegisterRequestHandler>();
        services.AddDatabase<UserServiceContext>(ServiceMetadata);
    }

    private static void Main() => Boot<ServiceBuilder<Program>>();
}