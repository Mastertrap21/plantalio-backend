using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Core.Extension;

public static class ServiceCollectionExtensions
{
    private static List<Action<IServiceProvider>>? _events = new();
    
    public static void AddDatabase<TContext>(this IServiceCollection services, ServiceMetadata.ServiceMetadata metadata) where TContext : DbContext 
    {
        services.AddScoped(factory =>
            factory.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext());
            
        services.AddDbContextFactory<TContext>(dbContextOptions =>
        {
            var connectionString = metadata.Database.Dsn;
            dbContextOptions
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
            
        // Auto migrate when a service provider is ready
        services.AddStartedEvent(serviceProvider =>
        {
            var contextName = typeof(TContext).Name;
            var log = serviceProvider.GetRequiredService<ILogger>();
            log.LogInformation("Migrating database context: {Context}", contextName);

            while (true)
            {
                try
                {
                    serviceProvider
                        .GetRequiredService<IDbContextFactory<TContext>>()
                        .CreateDbContext()
                        .Database
                        .Migrate();

                    log.LogInformation("Database context migration completed: {Context}", contextName);
                    break;
                }
                catch (Exception e)
                {
                    if (e is MySqlException {Number: 1042})
                    {
                        log.LogInformation("Unable to connect to database, trying again in 5 seconds..");
                        Thread.Sleep(5000);
                        continue;
                    }
                        
                    log.LogCritical(e, "Failed to migrate database context: {Context}", contextName);
                    throw;
                }
            }
        });
    }

    private static void AddStartedEvent(this IServiceCollection serviceCollection,
        Action<IServiceProvider> evt)
    {
        _events?.Add(evt);
    }

    public static void RaiseStartedEvent(this IServiceProvider? serviceProvider)
    {
        _events?.ForEach(e => e(serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider))));
        _events = null;
    }
}