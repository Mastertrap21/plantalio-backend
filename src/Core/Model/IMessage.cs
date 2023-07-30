using System.Collections.Generic;

namespace Core.Model;

public interface IMessage
{
    IList<ProcessPayload>? Payloads { get; set; }
    bool Prioritized { get; set; }
}