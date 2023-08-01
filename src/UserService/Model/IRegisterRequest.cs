using Core.Model;

namespace UserService.Model;

public interface IRegisterRequest : IFunctionPayload
{
    string? Username { get; set; }
    string? Password { get; set; }
}