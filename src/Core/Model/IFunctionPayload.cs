using System;

namespace Core.Model;

public interface IFunctionPayload
{
    string? RequestId { get; set; }
    ServiceName? ResponseService { get; set; }
}