namespace Core.Model;

public interface IFunctionMessage
{
    string? Function { get; set; }
    string? Payload { get; set; }
}