using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    public async Task<IActionResult> GetAllStories()
    {
        var stories = await _context.Stories
            .Include(s => s.Attachments)
            .Include(s => s.Comments)
            .Include(s => s.AssignedTo)
            .ToListAsync();

        var result = stories.Select(s => new {
            s.Id,
            s.Name,
            s.Description,
            Status = s.Status.ToString(),
            s.Priority,
            s.AssignedToId,
            AssignedTo = s.AssignedTo != null ? new { s.AssignedTo.Id, s.AssignedTo.Name } : null,
            AttachmentCount = s.Attachments?.Count ?? 0,
            CommentCount = s.Comments?.Count ?? 0
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStory(int id)
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
            .FirstOrDefaultAsync(s => s.Id == id);

        if (story == null) return NotFound();

        var response = new
        {
            story.Id,
            story.Name,
            story.Description,
            Status = story.Status.ToString(),
            story.Priority,
            story.AssignedToId,
            AssignedTo = story.AssignedTo != null ? new { story.AssignedTo.Id, story.AssignedTo.Name } : null,
            Attachments = story.Attachments.Select(a => new
            {
                a.Id,
                a.FileName,
                a.FilePath,
                a.ContentType,
                a.Size,
                a.UploadedAt
            }),
            Comments = story.Comments.Select(c => new
            {
                c.Id,
                Text = c.Text,
                c.CreatedAt,
                User = c.User != null ? new { c.User.Id, c.User.Name } : null,
                Attachments = c.Attachments.Select(a => new
                {
                    a.Id,
                    a.FileName,
                    a.FilePath,
                    a.ContentType,
                    a.Size,
                    a.UploadedAt
                })
            }),
            Tasks = story.Tasks.Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                Attachments = t.Attachments.Select(a => new
                {
                    a.Id,
                    a.FileName,
                    a.FilePath,
                    a.ContentType,
                    a.Size,
                    a.UploadedAt
                })
            })
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStory([FromBody] Story story)
    {
        _context.Stories.Add(story);
        await _context.SaveChangesAsync();
        return Ok(story);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStoryStatus(int id, [FromBody] StoryStatus status)
    {
        var story = await _context.Stories.FindAsync(id);
        if (story == null) return NotFound();
        story.Status = status;
        await _context.SaveChangesAsync();
        return Ok(story);
    }
}
