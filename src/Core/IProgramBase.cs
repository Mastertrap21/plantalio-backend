using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public interface IProgramBase
{
    IServiceProvider? ServiceProvider { get; set; }
    
    void ConfigureServices(IServiceCollection services);
    void Start();
    void Stop();
}