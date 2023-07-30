using System;
using System.Collections.Generic;

namespace Core.ServiceMetadata;

public abstract class ServiceMetadataBase
{
    private readonly Dictionary<string, object?> _metadata = new();
    
    protected T? Get<T>(string key, T? defaultValue = default)
    {
        try
        {
            if (_metadata.TryGetValue(key, out var foundValue))
            {
                return (T?) foundValue;
            }

            var value = defaultValue;
            var environmentValue = Environment.GetEnvironmentVariable(key);
            if (environmentValue != null)
            {
                value = (T) Convert.ChangeType(environmentValue, typeof(T));
            }

            _metadata.Add(key, value);
            return value;
        }
        catch (Exception e)
        {
            throw new Exception($"Cannot read metadata: {key}", e);
        }
    }
}