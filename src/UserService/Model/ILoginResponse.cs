using UserService.DTO;

namespace UserService.Model;

public interface ILoginResponse
{
    bool Success { get; set; }
    UserDTO? User { get; set; }
}