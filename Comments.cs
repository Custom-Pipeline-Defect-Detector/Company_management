public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int UserId { get; set; }
    public User? User { get; set; }
    public int? StoryId { get; set; }
    public Story? Story { get; set; }
    public int? TaskId { get; set; }
    public WorkTask? Task { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Attachment> Attachments { get; set; } = new();
}