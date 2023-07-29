using System;

namespace WebServer.Model;

public class Plant
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
}