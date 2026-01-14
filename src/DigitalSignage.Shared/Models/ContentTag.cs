namespace DigitalSignage.Shared.Models;

/// <summary>
/// Join entity for Content and Tag many-to-many relationship
/// </summary>
public class ContentTag
{
    public int ContentId { get; set; }
    public Content Content { get; set; } = null!;
    
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
