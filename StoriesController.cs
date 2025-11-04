using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/stories")]
public class StoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public StoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StorySummaryDto>>> GetAllStories()
    {
        var stories = await _context.Stories
            .Include(s => s.Attachments)
            .Include(s => s.Comments)
            .Include(s => s.AssignedTo)
            .AsNoTracking()
            .ToListAsync();

        return stories.Select(MapToSummary).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StoryDetailDto>> GetStory(int id)
    {
        var story = await _context.Stories
            .Include(s => s.Attachments)
            .Include(s => s.Comments)
                .ThenInclude(c => c.Attachments)
            .Include(s => s.Comments)
                .ThenInclude(c => c.User)
            .Include(s => s.Tasks)
                .ThenInclude(t => t.Attachments)
            .Include(s => s.AssignedTo)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (story == null)
        {
            return NotFound();
        }

        return MapToDetail(story);
    }

    [HttpPost]
    public async Task<ActionResult<StoryDetailDto>> CreateStory([FromBody] Story story)
    {
        _context.Stories.Add(story);
        await _context.SaveChangesAsync();

        var savedStory = await _context.Stories
            .Include(s => s.Attachments)
            .Include(s => s.Comments)
                .ThenInclude(c => c.Attachments)
            .Include(s => s.Comments)
                .ThenInclude(c => c.User)
            .Include(s => s.Tasks)
                .ThenInclude(t => t.Attachments)
            .Include(s => s.AssignedTo)
            .AsNoTracking()
            .FirstAsync(s => s.Id == story.Id);

        return CreatedAtAction(nameof(GetStory), new { id = savedStory.Id }, MapToDetail(savedStory));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStoryStatus(int id, [FromBody] StoryStatus status)
    {
        var story = await _context.Stories.FindAsync(id);
        if (story == null) return NotFound();
        story.Status = status;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static StorySummaryDto MapToSummary(Story story) => new(
        story.Id,
        story.Name,
        story.Description,
        story.Status.ToString(),
        story.Priority,
        story.AssignedToId,
        story.AssignedTo != null ? new UserSummaryDto(story.AssignedTo.Id, story.AssignedTo.Name) : null,
        story.Attachments?.Count ?? 0,
        story.Comments?.Count ?? 0);

    private static StoryDetailDto MapToDetail(Story story) => new(
        story.Id,
        story.Name,
        story.Description,
        story.Status.ToString(),
        story.Priority,
        story.AssignedToId,
        story.AssignedTo != null ? new UserSummaryDto(story.AssignedTo.Id, story.AssignedTo.Name) : null,
        story.Attachments?.Select(MapAttachment).ToList() ?? new List<AttachmentDto>(),
        story.Comments?.Select(MapComment).ToList() ?? new List<CommentDto>(),
        story.Tasks?.Select(MapTask).ToList() ?? new List<TaskDto>());

    private static AttachmentDto MapAttachment(Attachment attachment) => new(
        attachment.Id,
        attachment.FileName,
        attachment.FilePath,
        attachment.ContentType,
        attachment.Size,
        attachment.UploadedAt);

    private static CommentDto MapComment(Comment comment) => new(
        comment.Id,
        comment.Text,
        comment.CreatedAt,
        comment.User != null ? new UserSummaryDto(comment.User.Id, comment.User.Name) : null,
        comment.Attachments?.Select(MapAttachment).ToList() ?? new List<AttachmentDto>());

    private static TaskDto MapTask(WorkTask task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Attachments?.Select(MapAttachment).ToList() ?? new List<AttachmentDto>());
}
