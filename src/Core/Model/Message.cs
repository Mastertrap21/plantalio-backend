using System.Collections.Generic;

namespace Core.Model;

public class Message : MessageBase
{
    public IList<ProcessPayload>? Payloads { get; set; }
    public bool Prioritized { get; set; }
}