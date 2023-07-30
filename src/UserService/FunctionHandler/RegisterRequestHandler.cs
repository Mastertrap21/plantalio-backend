using Core.Messaging;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Entity;
using UserService.Model;
using BCryptNet = BCrypt.Net.BCrypt;

namespace UserService.FunctionHandler;

internal class RegisterRequestHandler : Core.Handler.FunctionHandler
{
    private readonly IProducer _producer;
    private readonly IDbContextFactory<UserServiceContext> _contextFactory;

    public RegisterRequestHandler(ILogger logger, IProducer producer, IDbContextFactory<UserServiceContext> contextFactory) : base(logger)
    {
        _producer = producer;
        _contextFactory = contextFactory;
    }

    public override void RegisterFuncListeners(IFunctionService? service)
    {
        service?.Register<RegisterRequest>(RegisterRequest);
    }

    private void RegisterRequest(RegisterRequest request)
    {
        var username = request.Username?.Trim();
        var response = new RegisterResponse();

        try
        {
            using var context = _contextFactory.CreateDbContext();
                
            Log.LogInformation("Handling register request. Checking username: {Username}", username);
                
            if (context.Users != null && context.Users.Any(u => u.Username == username))
            {
                response.Error = ErrorCodes.UserExists;
            }
            else
            {
                context.Users?.Add(new UserEntity
                {
                    UserId = Guid.NewGuid(),
                    Username = username,
                    Password = BCryptNet.HashPassword(request.Password),
                });
                context.SaveChanges();
                    
                response.Success = true;
            }
        }
        catch (Exception e)
        {
            Log.LogError(e, "Failed to create user. Request: {@Request}", request);
        }

        _producer.Respond(request, response);
    }
}