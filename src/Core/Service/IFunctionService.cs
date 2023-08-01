using System;
using Core.Model;

namespace Core.Service;

public interface IFunctionService
{
    void Start(string? serviceName);

    void Stop();
        
    void Register<T>(Action<T> action) where T : FunctionPayload;
    void RegisterAnyHandler(Action<IAnyFunctionPayload> action);
    void StartSubscriber(string? service);

}