namespace DigitalSignage.Shared.Models;

/// <summary>
/// Template entity for content decoration/styling
/// </summary>
public class Template
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CssClass { get; set; } = string.Empty;
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public string? FontFamily { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
