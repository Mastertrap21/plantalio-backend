using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ServiceBuilder<TProgram> : ServiceBuilderBase<TProgram>
    where TProgram : IProgramBase, new()
{
    private readonly IHostBuilder _builder = Host.CreateDefaultBuilder();
    private IHost _host = default!;

    public override IServiceBuilder Configure()
    {
        _builder
            .ConfigureServices(Program.ConfigureServices)
            .UseConsoleLifetime();
        return this;
    }

    public override IServiceBuilder Build()
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
            throw new Exception("Failed to stop service within timeout");
        }
        _host.Dispose();
    }
}