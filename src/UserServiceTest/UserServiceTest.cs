using System;
using System.Collections.Generic;
using Core.Messaging;
using Core.Model;
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
    private ILoginRequestHandler _loginRequestHandler;
    private IRegisterRequestHandler _registerRequestHandler;
    private IProducer _producer;
    private IFunctionPayload _lastRequest;
    private IFunctionPayload _lastResponse;

    [SetUp]
    public void Setup()
    {
        ILogger logger = new Mock<ILogger>().Object;
        var mock = new Mock<IProducer>();
        mock.Setup(p => p.Respond(It.IsAny<IFunctionPayload>(), It.IsAny<IFunctionPayload>()))
            .Callback((IFunctionPayload request, IFunctionPayload response) =>
            {
                _lastRequest = request;
                _lastResponse = response;
            });
        _producer = mock.Object;

        UserServiceContext context = new UserServiceContext(new DbContextOptionsBuilder<UserServiceContext>().UseInMemoryDatabase(databaseName: "PlantServiceTest").Options) { Users = GetQueryableMockDbSet(new List<UserEntity>
        {
            new UserEntity { Username = "Test", UserId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), Password = "$2y$10$KObsPVxQ2lFpeyHrQNPH1u7tmsVUM9GQt.x.LA9zRHI0hRw6/TaVW" }
        }) };
        var contextFactoryMock = new Mock<IDbContextFactory<UserServiceContext>>();
        contextFactoryMock.Setup(ps => ps.CreateDbContext()).Returns(() => context);
        IDbContextFactory<UserServiceContext> userServiceContextFactory = contextFactoryMock.Object;
        _loginRequestHandler = new LoginRequestHandler(logger, _producer, userServiceContextFactory);
        _registerRequestHandler = new RegisterRequestHandler(logger, _producer, userServiceContextFactory);
    }

    [Test]
    public void Test_AlwaysPass()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Test_Login_UserNotFound_Fail()
    {
        ILoginRequest request = new LoginRequest { Username = "NotTest", Password = "notTest" };
        _loginRequestHandler.Login(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(LoginResponse), _lastResponse);
        Assert.IsFalse(((LoginResponse)_lastResponse).Success);
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
    
}