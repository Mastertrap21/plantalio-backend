namespace Core.Model;

public class FunctionMessage : MessageBase
{
    public string? Function { get; set; }
    public string? Payload  { get; set; }
}