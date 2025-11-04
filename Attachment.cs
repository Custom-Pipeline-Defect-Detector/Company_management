public class Attachment
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public int? StoryId { get; set; }
    public Story? Story { get; set; }
    public int? TaskId { get; set; }
    public WorkTask? Task { get; set; }
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }
    public DateTime UploadedAt { get; set; }
}
