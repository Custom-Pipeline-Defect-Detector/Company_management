public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public List<Story> Stories { get; set; } = new();
    public List<WorkTask> Tasks { get; set; } = new();
}
