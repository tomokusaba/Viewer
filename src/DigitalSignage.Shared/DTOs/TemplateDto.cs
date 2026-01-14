namespace DigitalSignage.Shared.DTOs;

public record TemplateDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string CssClass { get; init; } = string.Empty;
    public string? BackgroundColor { get; init; }
    public string? TextColor { get; init; }
    public string? FontFamily { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateTemplateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string CssClass { get; init; } = string.Empty;
    public string? BackgroundColor { get; init; }
    public string? TextColor { get; init; }
    public string? FontFamily { get; init; }
    public bool IsActive { get; init; } = true;
}

public record UpdateTemplateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string CssClass { get; init; } = string.Empty;
    public string? BackgroundColor { get; init; }
    public string? TextColor { get; init; }
    public string? FontFamily { get; init; }
    public bool IsActive { get; init; }
}
