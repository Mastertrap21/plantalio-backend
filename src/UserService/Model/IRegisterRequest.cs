namespace UserService.Model;

public interface IRegisterRequest
{
    string? Username { get; set; }
    string? Password { get; set; }
}