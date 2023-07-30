namespace UserService.Entity;

public class UserEntity
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}