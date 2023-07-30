using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controller;

public interface IPlantController
{
    IEnumerable<IPlant> GetPlants();
    ActionResult GetPlant(Guid id);
    ActionResult AddPlant([FromBody]IPlant plant);
}