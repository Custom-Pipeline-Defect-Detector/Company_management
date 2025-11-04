public class Sprint
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public List<Story> Stories { get; set; } = new();
    public bool IsActive { get; set; }
}
