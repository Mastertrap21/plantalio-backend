using System;
using System.Collections.Generic;
using System.Linq;
using Core.Messaging;
using Core.Model;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PlantService.FunctionHandler;
using Moq;
using PlantService;
using PlantService.Entity;
using PlantService.Model;

namespace PlantServiceTest;

public class PlantServiceTest : TestCore.TestCoreTest
{
    private IGetPlantRequestHandler _getPlantRequestHandler;
    private IProducer _producer;
    private IFunctionPayload _lastRequest;
    private IFunctionPayload _lastResponse;
    private List<PlantEntity> _plantEntities;
    private Mock<ILogger> _loggerMock;
    private ILogger _logger;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>();
        _loggerMock.Setup(l => l.LogError(It.IsAny<Exception>(), "Failed to handle get plant request. Request: {@Request}", It.IsAny<object>())).Verifiable();
        _logger = _loggerMock.Object;
        var mock = new Mock<IProducer>();
        mock.Setup(p => p.Respond(It.IsAny<IFunctionPayload>(), It.IsAny<IFunctionPayload>()))
            .Callback((IFunctionPayload request, IFunctionPayload response) =>
            {
                _lastRequest = request;
                _lastResponse = response;
            });
        _producer = mock.Object;
        _plantEntities = new List<PlantEntity>
        {
            new PlantEntity { Name = "", PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") }
        };

        PlantServiceContext context = new PlantServiceContext(new DbContextOptionsBuilder<PlantServiceContext>().Options) { Plants = GetQueryableMockDbSet(_plantEntities) };
        var contextFactoryMock = new Mock<IDbContextFactory<PlantServiceContext>>();
        contextFactoryMock.Setup(ps => ps.CreateDbContext()).Returns(() => context);
        IDbContextFactory<PlantServiceContext> plantServiceContextFactory = contextFactoryMock.Object;
        _getPlantRequestHandler = new GetPlantRequestHandler(_logger, _producer, plantServiceContextFactory);
    }

    [Test]
    public void Test_AlwaysPass()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Test_RegisterFuncListeners_Exceptions()
    {
        ILogger logger = new Mock<ILogger>().Object;
        IMessageService messageService = new Mock<IMessageService>().Object;
        FunctionService fs = new FunctionService(logger, messageService);
        Assert.DoesNotThrow(() => _getPlantRequestHandler.RegisterFuncListeners(fs));
        Assert.Throws<Exception>(() => _getPlantRequestHandler.RegisterFuncListeners(fs));
    }
    
    [Test]
    public void Test_GetPlant_NotFound()
    {
        IGetPlantRequest request = new GetPlantRequest { PlantId = new Guid() };
        _getPlantRequestHandler.GetPlant(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(GetPlantResponse), _lastResponse);
        Assert.IsNull(((GetPlantResponse)_lastResponse).Plant?.PlantId);
    }
    
    [Test]
    public void Test_GetPlant_Found()
    {
        IGetPlantRequest request = new GetPlantRequest { PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") };
        _getPlantRequestHandler.GetPlant(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(GetPlantResponse), _lastResponse);
        Assert.IsNotNull(((GetPlantResponse)_lastResponse).Plant);
        Assert.IsNotNull(((GetPlantResponse)_lastResponse).Plant?.PlantId);
        Assert.AreEqual(((GetPlantResponse)_lastResponse).Plant?.PlantId, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"));
    }
    
    [Test]
    public void Test_GetPlant_DuplicateException()
    {
        PlantEntity plant = _plantEntities.First(p => p.PlantId == new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"));
        _plantEntities.Add(plant);
        IGetPlantRequest request = new GetPlantRequest { PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") };
        _getPlantRequestHandler.GetPlant(request);
        Assert.AreEqual(request, _lastRequest);
        Assert.IsInstanceOf(typeof(GetPlantResponse), _lastResponse);
        Assert.IsNull(((GetPlantResponse)_lastResponse).Plant);
        Assert.IsNull(((GetPlantResponse)_lastResponse).Plant?.PlantId);
        _loggerMock.VerifyAll();
        _plantEntities.RemoveAt(_plantEntities.Count - 1);
    }

}