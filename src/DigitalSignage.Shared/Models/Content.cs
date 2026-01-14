namespace DigitalSignage.Shared.Models;

/// <summary>
/// Content entity for digital signage
/// </summary>
public class Content
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ContentType Type { get; set; }
    public string? FilePath { get; set; }
    public string? MarkdownContent { get; set; }
    public string? Url { get; set; }
    public int? TemplateId { get; set; }
    public Template? Template { get; set; }
    public int Priority { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
}
