using System;
using System.Collections.Generic;

namespace Core.Model;

public interface IProcessPayload
{
    Guid Id { get; set; }
    List<Guid>? ParentIds { get; set; }
    PayloadData Data { get; set; }
}