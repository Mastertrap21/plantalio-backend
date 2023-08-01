using Core.Model;

namespace UserService.Model;

public interface ILoginRequest : IFunctionPayload
{
    string? Username { get; set; }
    string? Password { get; set; }
}