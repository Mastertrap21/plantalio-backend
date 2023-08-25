using System;
using Core.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ServiceBuilder<TProgram> : ServiceBuilderBase<TProgram>, IServiceBuilder where TProgram : IProgramBase, new()
{
    private readonly IHostBuilder _builder = Host.CreateDefaultBuilder();
    private IHost _host = default!;

    public override IServiceBuilderBase Configure()
    {
        _builder
            .ConfigureServices(Program.ConfigureServices)
            .UseConsoleLifetime();
        return this;
    }

    public override IServiceBuilderBase Build()
    {
        _host = _builder.Build();
        return this;
    }

    public override void Start()
    {
        Program.ServiceProvider = _host.Services;
            
        Program.ServiceProvider
            .GetRequiredService<IHostApplicationLifetime>()
            .ApplicationStopping.Register(() =>
            {
                Program.ServiceProvider
                    .GetService<ILogger>()
                    ?.LogInformation("SIGTERM signal received");

                Stop();
            });
            
        _host.Start();
        Program.Start();
    }

    public override void Stop()
    {
        Program.Stop();
        if (!_host.StopAsync().Wait(60000))
        {
            throw new Exception(ExceptionMessageTemplates.ServiceBuilderStopServiceWithinTimeoutFailed);
        }
        _host.Dispose();
    }
}