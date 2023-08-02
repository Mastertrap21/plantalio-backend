using Core.Handler;
using UserService.Model;

namespace UserService.FunctionHandler;

internal interface IRegisterRequestHandler : IFunctionHandler
{
    void Register(IRegisterRequest request);
}