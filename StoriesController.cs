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
            .ThenInclude(c => c.Attachments)
            .Include(s => s.AssignedTo)
            .ToListAsync();

        var result = stories.Select(s => new {
            s.Id,
            s.Name,
            s.Description,
            Status = s.Status,
            s.Priority,
            s.AssignedToId,
            AssignedTo = s.AssignedTo != null ? new { s.AssignedTo.Id, s.AssignedTo.Name } : null,
            AttachmentCount = s.Attachments?.Count ??0,
            CommentCount = s.Comments?.Count ??0
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetStory(int id)
    {
        var story = _context.Stories
            .Include(s => s.Attachments)
            .Include(s => s.Comments)
            .ThenInclude(c => c.Attachments)
            .FirstOrDefault(s => s.Id == id);
        if (story == null) return NotFound();
        return Ok(story);
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
