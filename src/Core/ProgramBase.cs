using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Extension;
using Core.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core;

public abstract class ProgramBase<T> : IProgramBase where T : ServiceMetadata.ServiceMetadata, new()
{
    protected T ServiceMetadata { get; }
    private readonly AutoResetEvent _autoResetEvent = new(false);
    
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ServiceMetadata.ServiceMetadata>(ServiceMetadata);
        services.AddSingleton(ServiceMetadata);
    }
    
    private IServiceProvider? _serviceProvider;

    public IServiceProvider? ServiceProvider
    {
        get => _serviceProvider;
        set
        {
            if (_serviceProvider != null)
            {
                throw new Exception("ServiceProvider was already set");
            }
                
            _serviceProvider = value;

            var version = typeof(IProgramBase).Assembly.GetName().Version ?? new Version();
            Logger.LogInformation("Running on core version: {Version}", string.Join('.', version.Major, version.Minor, version.Build));
            Logger.LogInformation("Service metadata values: {@Metadata}", ServiceMetadata.GetAll());
            _serviceProvider.RaiseStartedEvent();
        }
    }
    protected ILogger Logger => (ServiceProvider ?? throw new InvalidOperationException("ServiceProvider not set")).GetRequiredService<ILogger>();
    
    protected ProgramBase()
    {
        ServiceMetadata = new T();
    }
    
    public virtual void Start()
    {
        Logger.LogInformation("Service started (PID: {Pid})", Environment.ProcessId);
        _autoResetEvent.WaitOne();
        Logger.LogInformation("Stopping service");
        try
        {
            if (!Task.Run(Stop).Wait(10_000))
            {
                Logger.LogError("Timeout occurred when trying to stop service");
            }
                
            Logger.LogInformation("Service stopped gracefully");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to stop service gracefully");
        }
    }

    public virtual void Stop()
    {
        _autoResetEvent.Set();
    }

    protected static void Boot<TServiceBuilder>()
        where TServiceBuilder : IServiceBuilder, new()
    {
        new TServiceBuilder()
            .Configure()
            .Build()
            .Start();
    }
}