public class WorkTask
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int StoryId { get; set; }
    public Story Story { get; set; } = null!;
    public List<Attachment> Attachments { get; set; } = new();
}
