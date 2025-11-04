public class Story
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int EpicId { get; set; }
    public Epic Epic { get; set; } = null!;
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public List<WorkTask> Tasks { get; set; } = new();
    public StoryStatus Status { get; set; }
    public int? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }
    public int Priority { get; set; }
    public List<Label> Labels { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Attachment> Attachments { get; set; } = new();
}

public enum StoryStatus
{
    Backlog,
    ToDo,
    InProgress,
    Done
}
