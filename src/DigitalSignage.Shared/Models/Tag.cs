namespace DigitalSignage.Shared.Models;

/// <summary>
/// Tag entity for content classification
/// </summary>
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
}
