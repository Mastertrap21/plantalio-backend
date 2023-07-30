using Core.Service;
using Microsoft.Extensions.Logging;

namespace Core.Handler;

public abstract class FunctionHandler : IFunctionHandler
{
    protected readonly ILogger Log;

    protected FunctionHandler(ILogger logger)
    {
        Log = logger;
    }

    public abstract void RegisterFuncListeners(IFunctionService? service);
}