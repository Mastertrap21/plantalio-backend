using Core.Handler;
using UserService.Model;

namespace UserService.FunctionHandler;

internal interface ILoginRequestHandler : IFunctionHandler
{
    void Login(ILoginRequest request);
}