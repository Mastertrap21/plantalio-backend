using System;
using System.Collections.Generic;
using System.Linq;
using Core;
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
    private List<PlantEntity> _plantEntities;

    [SetUp]
    public void Setup()
    {
        _plantEntities = new List<PlantEntity>
        {
            new PlantEntity { Name = "", PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") }
        };

        PlantServiceContext context = new PlantServiceContext(new DbContextOptionsBuilder<PlantServiceContext>().Options) { Plants = GetQueryableMockDbSet(_plantEntities) };
        var contextFactoryMock = new Mock<IDbContextFactory<PlantServiceContext>>();
        contextFactoryMock.Setup(ps => ps.CreateDbContext()).Returns(() => context);
        IDbContextFactory<PlantServiceContext> plantServiceContextFactory = contextFactoryMock.Object;
        _getPlantRequestHandler = new GetPlantRequestHandler(Logger, Producer, plantServiceContextFactory);
    }

    [Test]
    public void Test_AlwaysPass()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Test_RegisterFuncListeners_Exceptions()
    {
        IMessageService messageService = new Mock<IMessageService>().Object;
        FunctionService fs = new FunctionService(Logger, messageService);
        Assert.DoesNotThrow(() => _getPlantRequestHandler.RegisterFuncListeners(fs));
        Assert.Throws<Exception>(() => _getPlantRequestHandler.RegisterFuncListeners(fs));
    }
    
    [Test]
    public void Test_ServiceStart_Success()
    {
        Mock<IMessageService> messageServiceMock = new Mock<IMessageService>();
        IMessageService messageService = messageServiceMock.Object;
        FunctionService fs = new FunctionService(Logger, messageService);
        _getPlantRequestHandler.RegisterFuncListeners(fs);
        fs.Start(ExecutionContext.Service);
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
        FunctionService fs = new FunctionService(Logger, messageService);
        _getPlantRequestHandler.RegisterFuncListeners(fs);
        fs.StartSubscriber(ExecutionContext.Service);
        messageServiceMock.Verify(
            ms => ms.Subscribe(
                It.IsAny<string>(), 
                It.IsAny<Action<FunctionMessage>>(), 
                It.IsAny<bool>()),
            Times.Once());
    }

    [Test]
    public void Test_GetPlant_NotFound()
    {
        IGetPlantRequest request = new GetPlantRequest { PlantId = new Guid() };
        _getPlantRequestHandler.GetPlant(request);
        Assert.AreEqual(request, LastRequest);
        Assert.IsInstanceOf(typeof(GetPlantResponse), LastResponse);
        Assert.IsNull(((GetPlantResponse)LastResponse).Plant?.PlantId);
    }
    
    [Test]
    public void Test_GetPlant_Found()
    {
        IGetPlantRequest request = new GetPlantRequest { PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") };
        _getPlantRequestHandler.GetPlant(request);
        Assert.AreEqual(request, LastRequest);
        Assert.IsInstanceOf(typeof(GetPlantResponse), LastResponse);
        Assert.IsNotNull(((GetPlantResponse)LastResponse).Plant);
        Assert.IsNotNull(((GetPlantResponse)LastResponse).Plant?.PlantId);
        Assert.AreEqual(((GetPlantResponse)LastResponse).Plant?.PlantId, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"));
    }
    
    [Test]
    public void Test_GetPlant_DuplicateException()
    {
        var plant = _plantEntities.First(p => p.PlantId == new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"));
        _plantEntities.Add(plant);
        IGetPlantRequest request = new GetPlantRequest { PlantId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") };
        _getPlantRequestHandler.GetPlant(request);
        Assert.AreEqual(request, LastRequest);
        Assert.IsInstanceOf(typeof(GetPlantResponse), LastResponse);
        Assert.IsNull(((GetPlantResponse)LastResponse).Plant);
        Assert.IsNull(((GetPlantResponse)LastResponse).Plant?.PlantId);
        VerifyLogger(LogLevel.Error, Times.Once());
        _plantEntities.RemoveAt(_plantEntities.Count - 1);
    }

}