using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controller;

[Route("api/[controller]")]
[ApiController]
public class PlantController : ControllerBase, IPlantController
{
    private static readonly ConcurrentBag<IPlant> Plants = new()
    {
        new Plant {
            Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
            Name = "Name",
            Species = "Species",
        }
    };
    [HttpGet()]
    public IEnumerable<IPlant> GetPlants()
    {
        return Plants.Select<IPlant, IPlant>(p => new Plant {
            Id = p.Id,
            Name = p.Name,
            Species = p.Species
        });
    }
    [HttpGet("{id:guid}")]
    public ActionResult GetPlant(Guid id)
    {
        IPlant? plant = Plants.SingleOrDefault(t => t.Id == id);
        if (plant == null) return NotFound();
        return new JsonResult(plant);
    }
    [HttpPost()]
    public ActionResult AddPlant([FromBody]IPlant plant)
    {
        plant.Id = Guid.NewGuid();
        Plants.Add(plant);
        return new JsonResult(plant);
    }
}