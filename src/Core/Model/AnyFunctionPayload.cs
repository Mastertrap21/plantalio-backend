using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Model;

public class AnyFunctionPayload : FunctionPayload, IAnyFunctionPayload
{
    public string? Json { get; }

    public AnyFunctionPayload(string? json)
    {
        Json = json;
    }

    public new string? RequestId
    {
        get
        {
            if (Json != null)
            {
                var deserializeObject = JsonConvert.DeserializeObject(Json);
                if (deserializeObject is JObject jobject)
                {
                    return jobject.TryGetValue(nameof(RequestId), out var value) ? value.Value<string>() : null;
                }
            }

            return null;
        }
    } 
}