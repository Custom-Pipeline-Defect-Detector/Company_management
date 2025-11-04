public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}

public enum UserRole
{
    Admin,
    Boss,
    Worker
}
