using System.Collections.Generic;
using System.Linq;

namespace CompanyManagement.Dtos;

public static class StoryMappingExtensions
{
    public static StorySummaryDto ToSummaryDto(this Story story)
    {
        return new StorySummaryDto(
            story.Id,
            story.Name,
            story.Description,
            story.Status.ToString(),
            story.Priority,
            story.AssignedToId,
            story.AssignedTo != null ? new UserSummaryDto(story.AssignedTo.Id, story.AssignedTo.Name) : null,
            story.Attachments?.Count ?? 0,
            story.Comments?.Count ?? 0);
    }

    public static StoryDetailDto ToDetailDto(this Story story)
    {
        return new StoryDetailDto(
            story.Id,
            story.Name,
            story.Description,
            story.Status.ToString(),
            story.Priority,
            story.AssignedToId,
            story.AssignedTo != null ? new UserSummaryDto(story.AssignedTo.Id, story.AssignedTo.Name) : null,
            story.Attachments.ToAttachmentDtos(),
            story.Comments.ToCommentDtos(),
            story.Tasks.ToTaskDtos());
    }

    private static IReadOnlyList<AttachmentDto> ToAttachmentDtos(this IEnumerable<Attachment>? attachments)
    {
        return attachments?
            .OrderBy(attachment => attachment.UploadedAt)
            .Select(attachment => new AttachmentDto(
                attachment.Id,
                attachment.FileName,
                attachment.FilePath,
                attachment.ContentType,
                attachment.Size,
                attachment.UploadedAt))
            .ToList() ?? new List<AttachmentDto>();
    }

    private static IReadOnlyList<CommentDto> ToCommentDtos(this IEnumerable<Comment>? comments)
    {
        return comments?
            .OrderBy(comment => comment.CreatedAt)
            .Select(comment => new CommentDto(
                comment.Id,
                comment.Text,
                comment.CreatedAt,
                comment.User != null ? new UserSummaryDto(comment.User.Id, comment.User.Name) : null,
                comment.Attachments.ToAttachmentDtos()))
            .ToList() ?? new List<CommentDto>();
    }

    private static IReadOnlyList<TaskDto> ToTaskDtos(this IEnumerable<WorkTask>? tasks)
    {
        return tasks?
            .OrderBy(task => task.Id)
            .Select(task => new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.Attachments.ToAttachmentDtos()))
            .ToList() ?? new List<TaskDto>();
    }
}
