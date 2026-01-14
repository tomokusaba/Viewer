namespace DigitalSignage.Shared.DTOs;

public record TagDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateTagDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public record UpdateTagDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
