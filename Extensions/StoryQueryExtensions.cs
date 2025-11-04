using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Extensions;

public static class StoryQueryExtensions
{
    public static IQueryable<Story> IncludeSummaryDependencies(this IQueryable<Story> query)
    {
        return query
            .Include(story => story.Attachments)
            .Include(story => story.Comments)
            .Include(story => story.AssignedTo);
    }

    public static IQueryable<Story> IncludeDetailDependencies(this IQueryable<Story> query)
    {
        return query
            .Include(story => story.Attachments)
            .Include(story => story.Comments)
                .ThenInclude(comment => comment.Attachments)
            .Include(story => story.Comments)
                .ThenInclude(comment => comment.User)
            .Include(story => story.Tasks)
                .ThenInclude(task => task.Attachments)
            .Include(story => story.AssignedTo);
    }
}
