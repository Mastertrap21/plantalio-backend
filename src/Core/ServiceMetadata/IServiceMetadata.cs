using System.Collections.Generic;

namespace Core.ServiceMetadata;

public interface IServiceMetadata
{
    Dictionary<string, object?> GetAll();
}