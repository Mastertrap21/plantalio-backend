using System;
using System.Collections.Generic;

namespace Core.Model;

public class ProcessPayload
{
    public Guid Id { get; set; } 
    public List<Guid>? ParentIds { get; set; } 
    public PayloadData Data { get; set; }

    public ProcessPayload(Guid payloadId)
    {
        Id = payloadId;
        Data = new PayloadData();
    }

    public ProcessPayload(Guid payloadId, PayloadData data) : this(payloadId)
    {
        Data = data;
    }
    public ProcessPayload(Guid payloadId, Dictionary<string, object> data) : this(payloadId)
    {
        Data = new PayloadData(data);
    }
    public ProcessPayload(Guid payloadId, ProcessPayload parent) : this(payloadId)
    {
        ParentIds = new List<Guid>()
        {
            parent.Id,
        };
        if (parent.ParentIds?.Count > 0)
        {
            ParentIds.AddRange(parent.ParentIds);
        }
    }
    public ProcessPayload(Guid payloadId, ProcessPayload parent, Dictionary<string, object> data) : this(payloadId, parent)
    {
        Data = new PayloadData(data);
    }
}