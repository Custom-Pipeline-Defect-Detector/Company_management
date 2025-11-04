using CompanyManagement.Dtos;
using CompanyManagement.Extensions;
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
            .IncludeSummaryDependencies()
            .AsNoTracking()
            .OrderBy(story => story.Id)
            .ToListAsync();

        return stories
            .Select(story => story.ToSummaryDto())
            .ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StoryDetailDto>> GetStory(int id)
    {
        var story = await _context.Stories
            .IncludeDetailDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (story == null)
        {
            return NotFound();
        }

        return story.ToDetailDto();
    }

    [HttpPost]
    public async Task<ActionResult<StoryDetailDto>> CreateStory([FromBody] Story story)
    {
        _context.Stories.Add(story);
        await _context.SaveChangesAsync();

        var savedStory = await _context.Stories
            .IncludeDetailDependencies()
            .AsNoTracking()
            .FirstAsync(s => s.Id == story.Id);

        return CreatedAtAction(nameof(GetStory), new { id = savedStory.Id }, savedStory.ToDetailDto());
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

}
