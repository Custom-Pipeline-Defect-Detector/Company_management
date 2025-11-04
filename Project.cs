using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Project
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int TeamId { get; set; }

    public Team Team { get; set; } = null!;

    // Initialize collections to avoid null reference exceptions.
    public List<Epic> Epics { get; set; } = new List<Epic>();
    public List<Sprint> Sprints { get; set; } = new List<Sprint>();
    public List<Story> Stories { get; set; } = new List<Story>();

    // Helper methods for working with collections
    public void AddEpic(Epic epic)
    {
        if (epic != null) Epics.Add(epic);
    }

    public void AddSprint(Sprint sprint)
    {
        if (sprint != null) Sprints.Add(sprint);
    }

    public void AddStory(Story story)
    {
        if (story != null) Stories.Add(story);
    }
}
