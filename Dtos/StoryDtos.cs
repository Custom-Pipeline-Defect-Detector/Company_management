using System;
using System.Collections.Generic;

public record UserSummaryDto(int Id, string Name);

public record AttachmentDto(
    int Id,
    string FileName,
    string FilePath,
    string ContentType,
    long Size,
    DateTime UploadedAt);

public record CommentDto(
    int Id,
    string Text,
    DateTime CreatedAt,
    UserSummaryDto? User,
    IReadOnlyList<AttachmentDto> Attachments);

public record TaskDto(
    int Id,
    string Title,
    string? Description,
    IReadOnlyList<AttachmentDto> Attachments);

public record StorySummaryDto(
    int Id,
    string Name,
    string Description,
    string Status,
    int Priority,
    int? AssignedToId,
    UserSummaryDto? AssignedTo,
    int AttachmentCount,
    int CommentCount);

public record StoryDetailDto(
    int Id,
    string Name,
    string Description,
    string Status,
    int Priority,
    int? AssignedToId,
    UserSummaryDto? AssignedTo,
    IReadOnlyList<AttachmentDto> Attachments,
    IReadOnlyList<CommentDto> Comments,
    IReadOnlyList<TaskDto> Tasks);
