using System;
using Core.Handler;
using Core.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.ProgramBase;

public class FunctionProgramBase : FunctionProgramBase<ServiceMetadata.ServiceMetadata>
{
}
public class FunctionProgramBase<T> : ProgramBase<T> where T : ServiceMetadata.ServiceMetadata, new()
{
    private IFunctionService? _service;

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<FunctionService>();
        services.AddSingleton<IFunctionService>(x => x.GetService<FunctionService>() ?? throw new InvalidOperationException("FunctionService not found"));
    }

    public override void Start()
    {
        _service = (ServiceProvider ?? throw new InvalidOperationException("ServiceProvider not set")).GetRequiredService<IFunctionService>();
        Logger.LogInformation("Starting service");
        try
        {
            foreach (var cmdHandler in ServiceProvider.GetServices<IFunctionHandler>())
            {
                cmdHandler.RegisterFuncListeners(_service);
            }
            _service.Start(ExecutionContext.Service);
            base.Start();
        }
        catch (Exception e)
        {
            Logger.LogCritical(e, "Failed to start service");
        }
    }

    public override void Stop()
    {
        _service?.Stop();
        base.Stop();
    }
}