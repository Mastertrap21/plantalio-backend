namespace UserService.Model;

public interface ILoginRequest
{
    string? Username { get; set; }
    string? Password { get; set; }
}