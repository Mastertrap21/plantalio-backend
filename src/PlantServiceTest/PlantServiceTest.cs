using System;
using System.Collections.Generic;
using Core.Messaging;
using Core.Model;
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

        PlantServiceContext context = new PlantServiceContext(new DbContextOptionsBuilder<PlantServiceContext>().Options) { Plants = GetQueryableMockDbSet(new List<PlantEntity>
        {
            new PlantEntity { Name = "", PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")}
        }) };
        var contextFactoryMock = new Mock<IDbContextFactory<PlantServiceContext>>();
        contextFactoryMock.Setup(ps => ps.CreateDbContext()).Returns(() => context);
        IDbContextFactory<PlantServiceContext> plantServiceContextFactory = contextFactoryMock.Object;
        _getPlantRequestHandler = new GetPlantRequestHandler(logger, _producer, plantServiceContextFactory);
    }

    [Test]
    public void Test_AlwaysPass()
    {
        Assert.Pass();
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
}