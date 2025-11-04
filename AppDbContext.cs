using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Team> Teams { get; set; } = null!;
	public DbSet<Project> Projects { get; set; } = null!;
	public DbSet<Epic> Epics { get; set; } = null!;
	public DbSet<Story> Stories { get; set; } = null!;
	public DbSet<WorkTask> Tasks { get; set; } = null!;
	public DbSet<Sprint> Sprints { get; set; } = null!;
	public DbSet<Label> Labels { get; set; } = null!;
	public DbSet<Comment> Comments { get; set; } = null!;
	public DbSet<Attachment> Attachments { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.HasOne(u => u.Team)
			.WithMany(t => t.Workers)
			.HasForeignKey(u => u.TeamId);

		modelBuilder.Entity<Team>()
			.HasOne(t => t.Boss)
			.WithMany()
			.HasForeignKey(t => t.BossId);

		modelBuilder.Entity<Project>()
			.HasOne(p => p.Team)
			.WithMany(t => t.Projects)
			.HasForeignKey(p => p.TeamId);

		modelBuilder.Entity<Epic>()
			.HasOne(e => e.Project)
			.WithMany(p => p.Epics)
			.HasForeignKey(e => e.ProjectId);

		modelBuilder.Entity<Story>()
			.HasOne(s => s.Epic)
			.WithMany(e => e.Stories)
			.HasForeignKey(s => s.EpicId);

		modelBuilder.Entity<Story>()
			.HasOne(s => s.Project)
			.WithMany(p => p.Stories)
			.HasForeignKey(s => s.ProjectId);

		modelBuilder.Entity<WorkTask>()
			.HasOne(t => t.Story)
			.WithMany(s => s.Tasks)
			.HasForeignKey(t => t.StoryId);

		modelBuilder.Entity<Sprint>()
			.HasOne(s => s.Project)
			.WithMany(p => p.Sprints)
			.HasForeignKey(s => s.ProjectId);

		modelBuilder.Entity<Comment>()
			.HasOne(c => c.User)
			.WithMany()
			.HasForeignKey(c => c.UserId);

		modelBuilder.Entity<Attachment>()
			.HasOne(a => a.Story)
			.WithMany(s => s.Attachments)
			.HasForeignKey(a => a.StoryId);

		modelBuilder.Entity<Attachment>()
			.HasOne(a => a.Task)
			.WithMany(t => t.Attachments)
			.HasForeignKey(a => a.TaskId);

		modelBuilder.Entity<Attachment>()
			.HasOne(a => a.Comment)
			.WithMany(c => c.Attachments)
			.HasForeignKey(a => a.CommentId);
	}
}
