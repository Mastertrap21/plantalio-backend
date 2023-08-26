using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Constants;
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
                throw new Exception(ExceptionMessageTemplates.ProgramBaseServiceProviderSetAlreadySet);
            }
                
            _serviceProvider = value;

            var version = typeof(IProgramBase).Assembly.GetName().Version ?? new Version();
            Logger.LogInformation(LoggingMessageTemplates.ProgramBaseServiceProviderSetRunningOnCoreVersion, string.Join('.', version.Major, version.Minor, version.Build));
            Logger.LogInformation(LoggingMessageTemplates.ProgramBaseServiceProviderSetServiceMetadataValues, ServiceMetadata.GetAll());
            _serviceProvider.RaiseStartedEvent();
        }
    }
    protected ILogger Logger => (ServiceProvider ?? throw new InvalidOperationException(ExceptionMessageTemplates.FunctionProgramBaseStartServiceProviderNotSet)).GetRequiredService<ILogger>();
    
    protected ProgramBase()
    {
        ServiceMetadata = new T();
    }
    
    public virtual void Start()
    {
        Logger.LogInformation(LoggingMessageTemplates.ProgramBaseStartServiceStarted, Environment.ProcessId);
        _autoResetEvent.WaitOne();
        Logger.LogInformation(LoggingMessageTemplates.ProgramBaseStartServiceStopping);
        try
        {
            if (!Task.Run(Stop).Wait(10_000))
            {
                Logger.LogError(LoggingMessageTemplates.ProgramBaseStartTimeoutOccuredWhenStoppingService);
            }
                
            Logger.LogInformation(LoggingMessageTemplates.ProgramBaseStartServiceStoppedGracefully);
        }
        catch (Exception e)
        {
            Logger.LogError(e, LoggingMessageTemplates.ProgramBaseStartServiceStopGracefullyFailed);
        }
    }

    public virtual void Stop()
    {
        _autoResetEvent.Set();
    }

    protected static void Boot<TServiceBuilder>()
        where TServiceBuilder : IServiceBuilderBase, new()
    {
        new TServiceBuilder()
            .Configure()
            .Build()
            .Start();
    }
}