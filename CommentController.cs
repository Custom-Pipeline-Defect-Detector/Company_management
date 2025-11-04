using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return Ok(comment);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.Attachments)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return NotFound();
        return Ok(comment);
    }
}
