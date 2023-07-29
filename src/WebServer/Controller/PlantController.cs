using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controller;

[Route("api/[controller]")]
[ApiController]
public class PlantController : ControllerBase
{
    private static ConcurrentBag<Plant> _plants = new ConcurrentBag<Plant> {
        new Plant {
            Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
            Name = "Name",
            Species = "Species",
        }
    };
    [HttpGet()]
    public IEnumerable GetPlants()
    {
        return _plants.Select(p => new {
            Id = p.Id,
            Name = p.Name,
            Species = p.Species
        });
    }
    [HttpGet("{id}")]
    public ActionResult GetPlant(Guid id)
    {
        var plant = _plants.SingleOrDefault(t => t.Id == id);
        if (plant == null) return NotFound();
 
        return new JsonResult(plant);
    }
    [HttpPost()]
    public ActionResult AddPlant([FromBody]Plant plant)
    {
        plant.Id = Guid.NewGuid();
        _plants.Add(plant);
        return new JsonResult(plant);
    }
}