namespace UserService.Entity;

public interface IUserEntity
{
    Guid UserId { get; set; }
    string? Username { get; set; }
    string? Password { get; set; }
}