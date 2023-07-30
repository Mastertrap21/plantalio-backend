using Core.Model;

namespace UserService.Model;

public class LoginRequest : FunctionPayload
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}