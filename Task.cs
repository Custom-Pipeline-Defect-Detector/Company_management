public class Task
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int StoryId { get; set; }
    public Story Story { get; set; }
    public TaskStatus Status { get; set; }
    public int? AssignedToId { get; set; }
    public User AssignedTo { get; set; }
    public int Priority { get; set; }
    public double Estimate { get; set; }
    public double TimeSpent { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Attachment> Attachments { get; set; }
}

public enum TaskStatus
{
    ToDo,
    InProgress,
    Done
}
