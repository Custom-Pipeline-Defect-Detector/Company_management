public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int BossId { get; set; }
    public User? Boss { get; set; }
    public List<User> Workers { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
}
