using Core.Model;
using UserService.DTO;

namespace UserService.Model;

public class LoginResponse : FunctionPayload, ILoginResponse
{
    public bool Success { get; set; }
    public UserDTO? User { get; set; }
}