using Microsoft.EntityFrameworkCore;
using DigitalSignage.Server.Data;
using DigitalSignage.Shared.DTOs;
using DigitalSignage.Shared.Models;

namespace DigitalSignage.Server.Endpoints;

public static class ContentEndpoints
{
    public static void MapContentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/contents").WithTags("Contents");

        group.MapGet("/", GetAllContents).WithName("GetAllContents");
        group.MapGet("/{id:int}", GetContentById).WithName("GetContentById");
        group.MapGet("/active", GetActiveContents).WithName("GetActiveContents");
        group.MapGet("/by-tags", GetContentsByTags).WithName("GetContentsByTags");
        group.MapPost("/", CreateContent).WithName("CreateContent");
        group.MapPut("/{id:int}", UpdateContent).WithName("UpdateContent");
        group.MapDelete("/{id:int}", DeleteContent).WithName("DeleteContent");
    }

    private static async Task<IResult> GetAllContents(ApplicationDbContext db)
    {
        var contents = await db.Contents
            .Include(c => c.Template)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .OrderByDescending(c => c.Priority)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync();

        var dtos = contents.Select(MapToDto).ToList();
        return Results.Ok(dtos);
    }

    private static async Task<IResult> GetContentById(int id, ApplicationDbContext db)
    {
        var content = await db.Contents
            .Include(c => c.Template)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (content is null)
            return Results.NotFound();

        return Results.Ok(MapToDto(content));
    }

    private static async Task<IResult> GetActiveContents(ApplicationDbContext db)
    {
        var now = DateTime.UtcNow;
        var contents = await db.Contents
            .Include(c => c.Template)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .Where(c => c.IsActive)
            .Where(c => !c.StartDate.HasValue || c.StartDate <= now)
            .Where(c => !c.EndDate.HasValue || c.EndDate >= now)
            .OrderByDescending(c => c.Priority)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync();

        var dtos = contents.Select(MapToDto).ToList();
        return Results.Ok(dtos);
    }

    private static async Task<IResult> GetContentsByTags(string tags, ApplicationDbContext db)
    {
        var tagIds = tags.Split(',').Select(int.Parse).ToList();
        var now = DateTime.UtcNow;

        var contents = await db.Contents
            .Include(c => c.Template)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .Where(c => c.IsActive)
            .Where(c => !c.StartDate.HasValue || c.StartDate <= now)
            .Where(c => !c.EndDate.HasValue || c.EndDate >= now)
            .Where(c => c.ContentTags.Any(ct => tagIds.Contains(ct.TagId)))
            .OrderByDescending(c => c.ContentTags.Count(ct => tagIds.Contains(ct.TagId)))
            .ThenByDescending(c => c.Priority)
            .ToListAsync();

        var dtos = contents.Select(MapToDto).ToList();
        return Results.Ok(dtos);
    }

    private static async Task<IResult> CreateContent(CreateContentDto dto, ApplicationDbContext db)
    {
        var content = new Content
        {
            Title = dto.Title,
            Description = dto.Description,
            Type = dto.Type,
            FilePath = dto.FilePath,
            MarkdownContent = dto.MarkdownContent,
            Url = dto.Url,
            TemplateId = dto.TemplateId,
            Priority = dto.Priority,
            IsActive = dto.IsActive,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Contents.Add(content);
        await db.SaveChangesAsync();

        // Add tags
        if (dto.TagIds.Count != 0)
        {
            foreach (var tagId in dto.TagIds)
            {
                db.ContentTags.Add(new ContentTag { ContentId = content.Id, TagId = tagId });
            }
            await db.SaveChangesAsync();
        }

        // Reload with includes
        var createdContent = await db.Contents
            .Include(c => c.Template)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .FirstAsync(c => c.Id == content.Id);

        return Results.Created($"/api/contents/{content.Id}", MapToDto(createdContent));
    }

    private static async Task<IResult> UpdateContent(int id, UpdateContentDto dto, ApplicationDbContext db)
    {
        var content = await db.Contents
            .Include(c => c.ContentTags)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (content is null)
            return Results.NotFound();

        content.Title = dto.Title;
        content.Description = dto.Description;
        content.Type = dto.Type;
        content.FilePath = dto.FilePath;
        content.MarkdownContent = dto.MarkdownContent;
        content.Url = dto.Url;
        content.TemplateId = dto.TemplateId;
        content.Priority = dto.Priority;
        content.IsActive = dto.IsActive;
        content.StartDate = dto.StartDate;
        content.EndDate = dto.EndDate;
        content.UpdatedAt = DateTime.UtcNow;

        // Update tags
        db.ContentTags.RemoveRange(content.ContentTags);
        foreach (var tagId in dto.TagIds)
        {
            db.ContentTags.Add(new ContentTag { ContentId = content.Id, TagId = tagId });
        }

        await db.SaveChangesAsync();

        // Reload with includes
        var updatedContent = await db.Contents
            .Include(c => c.Template)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .FirstAsync(c => c.Id == content.Id);

        return Results.Ok(MapToDto(updatedContent));
    }

    private static async Task<IResult> DeleteContent(int id, ApplicationDbContext db)
    {
        var content = await db.Contents.FindAsync(id);
        if (content is null)
            return Results.NotFound();

        db.Contents.Remove(content);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static ContentDto MapToDto(Content content)
    {
        return new ContentDto
        {
            Id = content.Id,
            Title = content.Title,
            Description = content.Description,
            Type = content.Type,
            FilePath = content.FilePath,
            MarkdownContent = content.MarkdownContent,
            Url = content.Url,
            TemplateId = content.TemplateId,
            Template = content.Template != null ? new TemplateDto
            {
                Id = content.Template.Id,
                Name = content.Template.Name,
                Description = content.Template.Description,
                CssClass = content.Template.CssClass,
                BackgroundColor = content.Template.BackgroundColor,
                TextColor = content.Template.TextColor,
                FontFamily = content.Template.FontFamily,
                IsActive = content.Template.IsActive,
                CreatedAt = content.Template.CreatedAt
            } : null,
            Priority = content.Priority,
            IsActive = content.IsActive,
            StartDate = content.StartDate,
            EndDate = content.EndDate,
            CreatedAt = content.CreatedAt,
            UpdatedAt = content.UpdatedAt,
            Tags = content.ContentTags.Select(ct => new TagDto
            {
                Id = ct.Tag.Id,
                Name = ct.Tag.Name,
                Description = ct.Tag.Description,
                CreatedAt = ct.Tag.CreatedAt
            }).ToList()
        };
    }
}
