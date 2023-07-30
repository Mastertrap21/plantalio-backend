using System;

namespace Core.Model;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Realname { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Country { get; set; }
    public string? Language { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}