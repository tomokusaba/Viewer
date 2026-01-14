namespace DigitalSignage.Shared.DTOs;

using DigitalSignage.Shared.Models;

public record ContentDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ContentType Type { get; init; }
    public string? FilePath { get; init; }
    public string? MarkdownContent { get; init; }
    public string? Url { get; init; }
    public int? TemplateId { get; init; }
    public TemplateDto? Template { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public List<TagDto> Tags { get; init; } = new();
}

public record CreateContentDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ContentType Type { get; init; }
    public string? FilePath { get; init; }
    public string? MarkdownContent { get; init; }
    public string? Url { get; init; }
    public int? TemplateId { get; init; }
    public int Priority { get; init; } = 0;
    public bool IsActive { get; init; } = true;
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<int> TagIds { get; init; } = new();
}

public record UpdateContentDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ContentType Type { get; init; }
    public string? FilePath { get; init; }
    public string? MarkdownContent { get; init; }
    public string? Url { get; init; }
    public int? TemplateId { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<int> TagIds { get; init; } = new();
}
