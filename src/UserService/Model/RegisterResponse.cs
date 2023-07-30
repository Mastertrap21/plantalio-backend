using Core.Model;

namespace UserService.Model;

public class RegisterResponse : FunctionPayload, IRegisterResponse
{
    public bool Success { get; set; }
    public ErrorCodes Error { get; set; }
}