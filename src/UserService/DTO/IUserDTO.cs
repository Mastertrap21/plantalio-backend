namespace UserService.DTO;

public interface IUserDTO
{
    Guid UserId { get; set; }
    string? Username { get; set; }
}