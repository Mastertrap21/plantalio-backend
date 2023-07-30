using System;

namespace Core.Model;

public class FunctionPayload : IFunctionPayload
{
    public string? RequestId { get; set; }
    public Guid? CustomerId { get; set; }
    public ServiceName? ResponseService { get; set; }
}