using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

[ApiController]
[Route("api/attachments")]
public class AttachmentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AttachmentsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAttachment([FromForm] IFormFile file, [FromForm] int? storyId, [FromForm] int? taskId, [FromForm] int? commentId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            FileName = file.FileName,
            FilePath = $"/uploads/{uniqueFileName}",
            ContentType = file.ContentType,
            Size = file.Length,
            StoryId = storyId,
            TaskId = taskId,
            CommentId = commentId,
            UploadedAt = DateTime.UtcNow
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return Ok(attachment);
    }

    [HttpGet("{id}")]
    public IActionResult GetAttachment(int id)
    {
        var attachment = _context.Attachments.Find(id);
        if (attachment == null)
            return NotFound();

        var relativePath = attachment.FilePath?.TrimStart('/') ?? string.Empty;
        var filePath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), relativePath);
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var fileStream = System.IO.File.OpenRead(filePath);
        return File(fileStream, attachment.ContentType, attachment.FileName);
    }
}
