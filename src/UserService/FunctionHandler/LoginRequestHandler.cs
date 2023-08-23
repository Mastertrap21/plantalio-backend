using Core.Messaging;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.DTO;
using UserService.Model;
using BCryptNet = BCrypt.Net.BCrypt;

namespace UserService.FunctionHandler;

internal class LoginRequestHandler : Core.Handler.FunctionHandler, ILoginRequestHandler
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
        service?.Register<LoginRequest>(Login);
    }

    public void Login(ILoginRequest request)
    {
        var username = request.Username;
        var response = new LoginResponse();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            
            Log.LogInformation(LoggingMessageTemplates.LoginRequestHandleUsernameCheck, username);
            
            if (username == null)
            {
                response.Error = ErrorCodes.MissingFields;
                _producer.Respond(request, response);
                return;
            }
            
            username = username.Trim();

            if (context.Users == null)
            {
                response.Error = ErrorCodes.UnknownError;
                Log.LogError(LoggingMessageTemplates.LoginRequestHandleUsersNullError, username);
                _producer.Respond(request, response);
                return;
            }

            var user = context.Users
                    .SingleOrDefault(u => u.Username == username);
            
            if (user == null)
            {
                response.Error = ErrorCodes.WrongUserOrPassword;
                Log.LogInformation(LoggingMessageTemplates.LoginRequestHandleAuthFailedError, username);
                _producer.Respond(request, response);
                return;
            }
            
            if(!BCryptNet.Verify(request.Password, user.Password))
            {
                response.Error = ErrorCodes.WrongUserOrPassword;
                Log.LogInformation(LoggingMessageTemplates.LoginRequestHandleAuthFailedError, username);
                _producer.Respond(request, response);
                return;
            }
            
            Log.LogInformation(LoggingMessageTemplates.LoginRequestHandleAuthSuccess, username);
            response.Success = true;
            response.User = new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username
            };
        }
        catch (Exception e)
        {
            response.Error = ErrorCodes.UnknownError;
            Log.LogError(e, LoggingMessageTemplates.LoginRequestHandleFailError, request);
        }

        _producer.Respond(request, response);
    }
}