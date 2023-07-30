using System;
using System.Reflection;

namespace Core;

public static class ExecutionContext
{
    public static readonly string? Service = Assembly.GetEntryAssembly()?.GetName().Name;
    public static Guid? ProcessId { get; set; }
}