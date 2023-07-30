namespace Core.Model;

public class FunctionMessage : MessageBase, IFunctionMessage
{
    public string? Function { get; set; }
    public string? Payload  { get; set; }
}