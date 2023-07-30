namespace Core.Model;

public interface IAnyFunctionPayload : IFunctionPayload
{
    string? Json { get; }
    new string? RequestId { get; }
}