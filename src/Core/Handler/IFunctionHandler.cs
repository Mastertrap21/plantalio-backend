using Core.Service;

namespace Core.Handler;

public interface IFunctionHandler
{
    void RegisterFuncListeners(IFunctionService? service);
}