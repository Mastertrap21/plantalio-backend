using System;
using System.Collections.Generic;
using Core;
using Core.Messaging;
using Core.Model;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UserService.FunctionHandler;
using Moq;
using UserService;
using UserService.Entity;
using UserService.Model;

namespace UserServiceTest;

public class UserServiceTest : TestCore.TestCoreTest
{
    private UserServiceContext _userServiceContext;
    private Mock<IDbContextFactory<UserServiceContext>> _userServiceContextFactoryMock;
    private DbContextOptions<UserServiceContext> _userServiceDbContextOptions;
    private DbSet<UserEntity> _userServiceDbSet;
    private ILoginRequestHandler _loginRequestHandler;
    private IRegisterRequestHandler _registerRequestHandler;
    private IProducer _producer;
    private IFunctionPayload _lastRequest;
    private IFunctionPayload _lastResponse;

    [SetUp]
    public void Setup()
    {
        var mock = new Mock<IProducer>();
        mock.Setup(p => p.Respond(It.IsAny<IFunctionPayload>(), It.IsAny<IFunctionPayload>()))
            .Callback((IFunctionPayload request, IFunctionPayload response) =>
            {
                _lastRequest = request;
                _lastResponse = response;
            });
        _producer = mock.Object;

        _userServiceDbContextOptions = new DbContextOptionsBuilder<UserServiceContext>()
            .UseInMemoryDatabase(databaseName: "PlantServiceTest").Options;
        _userServiceDbSet = GetQueryableMockDbSet(new List<UserEntity>
        {
            new UserEntity
            {
                Username = "Test", UserId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                Password = "$2y$10$KObsPVxQ2lFpeyHrQNPH1u7tmsVUM9GQt.x.LA9zRHI0hRw6/TaVW"
            }
        });
        _userServiceContext = new UserServiceContext(_userServiceDbContextOptions) { Users =  _userServiceDbSet };
        _userServiceContextFactoryMock = new Mock<IDbContextFactory<UserServiceContext>>();
        _userServiceContextFactoryMock.Setup(ps => ps.CreateDbContext()).Returns(() => _userServiceContext);
        var userServiceContextFactory = _userServiceContextFactoryMock.Object;
        _loginRequestHandler = new LoginRequestHandler(Logger, _producer, userServiceContextFactory);
        _registerRequestHandler = new RegisterRequestHandler(Logger, _producer, userServiceContextFactory);
    }

    [Test]
    public void Test_AlwaysPass()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Test_PlantServiceContextEmptyConstructor()
    {
        var context = new TestableUserServiceContext();
        Assert.IsInstanceOf(typeof(UserServiceContext), context);
    }
    
    [Test]
    public void Test_PlantServiceContextOnModelCreating()
    {
        DbContextOptionsBuilder<UserServiceContext> builder = new DbContextOptionsBuilder<UserServiceContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        var options = builder.Options;
        var context = new TestableUserServiceContext(options);
        context.Database.EnsureCreated();
        var entityTypeBuilder = context.GetEntityTypeBuilder();
        
        var userIdColumn = entityTypeBuilder?.Metadata.FindDeclaredProperty(nameof(UserEntity.UserId));
        Assert.NotNull(userIdColumn);
        Assert.False(userIdColumn.IsNullable);
        Assert.True(userIdColumn.IsKey());
        
        var usernameColumn = entityTypeBuilder.Metadata.FindDeclaredProperty(nameof(UserEntity.Username));
        Assert.NotNull(usernameColumn);
        Assert.False(usernameColumn.IsNullable);
        Assert.False(usernameColumn.IsKey());
        
        var passwordColumn = entityTypeBuilder.Metadata.FindDeclaredProperty(nameof(UserEntity.Password));
        Assert.NotNull(passwordColumn);
        Assert.False(passwordColumn.IsNullable);
    }
    
    [Test]
    public void Test_RegisterFuncListeners_Exceptions()
    {
        IMessageService messageService = new Mock<IMessageService>().Object;
        FunctionService fs = new FunctionService(Logger, messageService);
        Assert.DoesNotThrow(() => _loginRequestHandler.RegisterFuncListeners(fs));
        Assert.Throws<Exception>(() => _loginRequestHandler.RegisterFuncListeners(fs));
        Assert.DoesNotThrow(() => _registerRequestHandler.RegisterFuncListeners(fs));
        Assert.Throws<Exception>(() => _registerRequestHandler.RegisterFuncListeners(fs));
    }

    [Test]
    public void Test_RegisterFuncListeners_Success()
    {
        Mock<IFunctionService> functionServiceMock = new Mock<IFunctionService>();
        IFunctionService functionService = functionServiceMock.Object;
         _loginRequestHandler.RegisterFuncListeners(functionService);
         functionServiceMock.Verify(
            fs => fs.Register(
                It.IsAny<Action<LoginRequest>>()),
            Times.Once());
         _registerRequestHandler.RegisterFuncListeners(functionService);
         functionServiceMock.Verify(
             fs => fs.Register(
                 It.IsAny<Action<RegisterRequest>>()),
             Times.Once());
    }
    
    [Test]
    public void Test_ServiceStart_Success()
    {
        Mock<IMessageService> messageServiceMock = new Mock<IMessageService>();
        IMessageService messageService = messageServiceMock.Object;
        Mock<FunctionService> functionServiceMock = new Mock<FunctionService>(Logger, messageService);
        IFunctionService functionService = functionServiceMock.Object;
        functionService.Start(ExecutionContext.Service);
        messageServiceMock.Verify(
            ms => ms.Subscribe(
                It.IsAny<string>(), 
                It.IsAny<Action<FunctionMessage>>(), 
                It.IsAny<bool>()),
            Times.Once());
    }
    
    [Test]
    public void Test_ServiceStartSubscriber_Success()
    {
        Mock<IMessageService> messageServiceMock = new Mock<IMessageService>();
        IMessageService messageService = messageServiceMock.Object;
        Mock<FunctionService> functionServiceMock = new Mock<FunctionService>(Logger, messageService);
        IFunctionService functionService = functionServiceMock.Object;
        functionService.StartSubscriber(ExecutionContext.Service);
        messageServiceMock.Verify(
            ms => ms.Subscribe(
                It.IsAny<string>(), 
                It.IsAny<Action<FunctionMessage>>(), 
                It.IsAny<bool>()),
            Times.Exactly(1));
    }
    
    [Test]
    public void Test_Login_UserNotFound_Fail()
    {
        ILoginRequest request = new LoginRequest { Username = "NotTest", Password = "notTest" };
        _loginRequestHandler.Login(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(LoginResponse), _lastResponse);
        Assert.IsFalse(((LoginResponse)_lastResponse).Success);
        Assert.IsNotNull(((LoginResponse)_lastResponse).Error);
        Assert.AreEqual(((LoginResponse)_lastResponse).Error, ErrorCodes.WrongUserOrPassword);
        Assert.IsNull(((LoginResponse)_lastResponse).User?.UserId);
    }
    
    [Test]
    public void Test_Login_WrongPassword_Fail()
    {
        ILoginRequest request = new LoginRequest { Username = "Test", Password = "notTest" };
        _loginRequestHandler.Login(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(LoginResponse), _lastResponse);
        Assert.IsFalse(((LoginResponse)_lastResponse).Success);
        Assert.IsNotNull(((LoginResponse)_lastResponse).Error);
        Assert.AreEqual(((LoginResponse)_lastResponse).Error, ErrorCodes.WrongUserOrPassword);
        Assert.IsNull(((LoginResponse)_lastResponse).User?.UserId);
    }

    [Test]
    public void Test_Login_Success()
    {
        ILoginRequest request = new LoginRequest { Username = "Test", Password = "test" };
        _loginRequestHandler.Login(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(LoginResponse), _lastResponse);
        Assert.IsTrue(((LoginResponse)_lastResponse).Success);
        Assert.IsNull(((LoginResponse)_lastResponse).Error);
        Assert.IsNotNull(((LoginResponse)_lastResponse).User);
        Assert.IsNotNull(((LoginResponse)_lastResponse).User?.UserId);
        Assert.AreEqual(((LoginResponse)_lastResponse).User?.UserId, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"));
    }
    
    [Test]
    public void Test_Register_UserExists_Fail()
    {
        IRegisterRequest request = new RegisterRequest { Username = "Test", Password = "test" };
        _registerRequestHandler.Register(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(RegisterResponse), _lastResponse);
        Assert.IsFalse(((RegisterResponse)_lastResponse).Success);
        Assert.IsNotNull(((RegisterResponse)_lastResponse).Error);
        Assert.AreEqual(((RegisterResponse)_lastResponse).Error, ErrorCodes.UserExists);
    }
    
    [Test]
    public void Test_Register_Success()
    {
        IRegisterRequest request = new RegisterRequest { Username = "Test2", Password = "test2" };
        _registerRequestHandler.Register(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(RegisterResponse), _lastResponse);
        Assert.IsTrue(((RegisterResponse)_lastResponse).Success);
        Assert.IsNull(((RegisterResponse)_lastResponse).Error);
    }
    
    [Test]
    public void Test_RegisterUsernameTrim_Success()
    {
        IRegisterRequest registerRequest = new RegisterRequest { Username = " TestTrim ", Password = "testTrim" };
        _registerRequestHandler.Register(registerRequest);
        Assert.AreEqual(registerRequest, _lastRequest);
        Assert.IsInstanceOf(typeof(RegisterResponse), _lastResponse);
        Assert.IsTrue(((RegisterResponse)_lastResponse).Success);
        Assert.IsNull(((RegisterResponse)_lastResponse).Error);
        
        ILoginRequest loginRequest = new LoginRequest { Username = "TestTrim", Password = "testTrim" };
        _loginRequestHandler.Login(loginRequest);
        Assert.AreEqual(loginRequest, _lastRequest);
        Assert.IsInstanceOf(typeof(LoginResponse), _lastResponse);
        Assert.IsTrue((_lastResponse as ILoginResponse)?.Success);
        Assert.IsNull((_lastResponse as ILoginResponse)?.Error);
        Assert.IsNotNull((_lastResponse as ILoginResponse)?.User);
        Assert.IsNotNull((_lastResponse as ILoginResponse)?.User?.UserId);
        Assert.AreEqual((_lastResponse as ILoginResponse)?.User?.Username, "TestTrim");
    }

    [Test]
    public void Test_RegisterNullUsername_Fail()
    {
        IRegisterRequest request = new RegisterRequest { Password = "testNullFail" };
        _registerRequestHandler.Register(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(RegisterResponse), _lastResponse);
        Assert.IsFalse(((RegisterResponse)_lastResponse).Success);
        Assert.IsNotNull(((RegisterResponse)_lastResponse).Error);
        Assert.AreEqual(((RegisterResponse)_lastResponse).Error, ErrorCodes.MissingFields);
    }
    
    [Test]
    public void Test_RegisterNullUsers_Fail()
    {
        _userServiceContext.Users = null;
        IRegisterRequest request = new RegisterRequest { Username = "Test2", Password = "test2" };
        _registerRequestHandler.Register(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(RegisterResponse), _lastResponse);
        Assert.IsFalse(((RegisterResponse)_lastResponse).Success);
        Assert.IsNotNull(((RegisterResponse)_lastResponse).Error);
        Assert.AreEqual(((RegisterResponse)_lastResponse).Error, ErrorCodes.UnknownError);
    }
    
    [Test]
    public void Test_RegisterException_Fail()
    {
        var userServiceContextMock = new Mock<UserServiceContext>(_userServiceDbContextOptions);
        var userServiceContext = userServiceContextMock.Object;
        userServiceContext.Users = _userServiceDbSet;
        userServiceContextMock.Setup(usc => usc.SaveChanges()).Callback(() => throw new Exception("Unit test exception"));
        _userServiceContextFactoryMock.Setup(ps => ps.CreateDbContext()).Returns(() => userServiceContextMock.Object);
        IRegisterRequest request = new RegisterRequest { Username = "Test2", Password = "test2" };
        _registerRequestHandler.Register(request);
        VerifyLogger(LogLevel.Error, Times.Once());
    }

}