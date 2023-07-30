namespace UserService.Entity;

public class UserEntity : IUserEntity
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}