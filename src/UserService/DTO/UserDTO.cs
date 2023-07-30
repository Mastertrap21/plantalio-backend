namespace UserService.DTO;

public class UserDTO : IUserDTO
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
}