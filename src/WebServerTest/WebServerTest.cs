using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebServer.Controller;
using WebServer.Model;

namespace WebServerTest;

public class WebServerTest : TestCore.TestCore
{
    
    private PlantController _plantController;

    [SetUp]
    public void Setup()
    {
        _plantController = new PlantController();
    }

    [Test]
    public void AddAndGetPlant_Works()
    {
        var newPlant = new Plant
        {
            Name = "Name",
            Species = "Species",
        };
        ActionResult<Plant> addPlantResult = _plantController.AddPlant(newPlant);
        Assert.AreEqual(newPlant, ((JsonResult)addPlantResult.Result)?.Value);

        ActionResult<Plant> getPlantResult = _plantController.GetPlant(newPlant.Id);
        Assert.AreEqual(newPlant, ((JsonResult)getPlantResult.Result)?.Value);
    }
}