using System;

namespace Core.Model;

public class Plant
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}