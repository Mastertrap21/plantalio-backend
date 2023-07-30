using System;

namespace WebServer.Model;

public interface IPlant
{
    Guid Id { get; set; }
    string? Name { get; set; }
    string? Species { get; set; }
}