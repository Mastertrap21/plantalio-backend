using Core.Messaging;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.DTO;
using UserService.Model;
using BCryptNet = BCrypt.Net.BCrypt;

namespace UserService.FunctionHandler;

internal class LoginRequestHandler : Core.Handler.FunctionHandler
{
    private readonly IProducer _producer;
    private readonly IDbContextFactory<UserServiceContext> _contextFactory;

    public LoginRequestHandler(ILogger logger, IProducer producer, IDbContextFactory<UserServiceContext> contextFactory) : base(logger)
    {
        _producer = producer;
        _contextFactory = contextFactory;
    }

    public override void RegisterFuncListeners(IFunctionService? service)
    {
        service?.Register<LoginRequest>(LoginRequest);
    }

    private void LoginRequest(LoginRequest request)
    {
        var username = request.Username?.Trim();
        var response = new LoginResponse();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            
            Log.LogInformation("Handling login request. Checking username: {Username}", username);
            if (context.Users != null)
            {
                var user = context.Users
                    .SingleOrDefault(u => u.Username == request.Username);
                if (user != null && BCryptNet.Verify(request.Password, user.Password))
                {
                    Log.LogInformation("Authentication succeeded for user: {Username}", username);
                    response.Success = true;
                    response.User = new UserDTO
                    {
                        UserId = user.UserId,
                        Username = user.Username
                    };
                }
                else
                {
                    Log.LogInformation("Authentication failed for user: {Username}", username);
                }
            }
        }
        catch (Exception e)
        {
            Log.LogError(e, "Failed to handle login request. Request: {@Request}", request);
        }

        _producer.Respond(request, response);
    }
}