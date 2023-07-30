using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.ServiceMetadata.Common;

namespace Core.ServiceMetadata;

public class ServiceMetadata : ServiceMetadataBase, IServiceMetadata
{
    public readonly Database Database = new();
    public Dictionary<string, object?> GetAll()
    {
        var metadata = new Dictionary<string, object?>();
        var type = GetType();
        foreach (var property in type.GetProperties().Concat<MemberInfo>(type.GetFields()))
        {
            object? value = null;
            if(property.GetType() == typeof(PropertyInfo))
            {
                value = ((PropertyInfo)property).GetValue(this);
            }
            else if(property.GetType() == typeof(FieldInfo))
            {
                value = ((FieldInfo)property).GetValue(this);
            }
            metadata.Add(property.Name, value);
        }
        return metadata;
    }
}