using Core.Model;

namespace UserService.Model;

public class RegisterRequest : FunctionPayload, IRegisterRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}