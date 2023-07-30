using System;

namespace Core.Model;

public interface IFunctionPayload
{
    string? RequestId { get; set; }
    Guid? CustomerId { get; set; }
    ServiceName? ResponseService { get; set; }
}