using Microsoft.EntityFrameworkCore;
using DigitalSignage.Server.Data;
using DigitalSignage.Shared.DTOs;
using DigitalSignage.Shared.Models;

namespace DigitalSignage.Server.Endpoints;

public static class TemplateEndpoints
{
    public static void MapTemplateEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/templates").WithTags("Templates");

        group.MapGet("/", GetAllTemplates).WithName("GetAllTemplates");
        group.MapGet("/{id:int}", GetTemplateById).WithName("GetTemplateById");
        group.MapGet("/active", GetActiveTemplates).WithName("GetActiveTemplates");
        group.MapPost("/", CreateTemplate).WithName("CreateTemplate");
        group.MapPut("/{id:int}", UpdateTemplate).WithName("UpdateTemplate");
        group.MapDelete("/{id:int}", DeleteTemplate).WithName("DeleteTemplate");
    }

    private static async Task<IResult> GetAllTemplates(ApplicationDbContext db)
    {
        var templates = await db.Templates
            .OrderBy(t => t.Name)
            .ToListAsync();

        var dtos = templates.Select(MapToDto).ToList();
        return Results.Ok(dtos);
    }

    private static async Task<IResult> GetTemplateById(int id, ApplicationDbContext db)
    {
        var template = await db.Templates.FindAsync(id);
        if (template is null)
            return Results.NotFound();

        return Results.Ok(MapToDto(template));
    }

    private static async Task<IResult> GetActiveTemplates(ApplicationDbContext db)
    {
        var templates = await db.Templates
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync();

        var dtos = templates.Select(MapToDto).ToList();
        return Results.Ok(dtos);
    }

    private static async Task<IResult> CreateTemplate(CreateTemplateDto dto, ApplicationDbContext db)
    {
        var template = new Template
        {
            Name = dto.Name,
            Description = dto.Description,
            CssClass = dto.CssClass,
            BackgroundColor = dto.BackgroundColor,
            TextColor = dto.TextColor,
            FontFamily = dto.FontFamily,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        db.Templates.Add(template);
        await db.SaveChangesAsync();

        return Results.Created($"/api/templates/{template.Id}", MapToDto(template));
    }

    private static async Task<IResult> UpdateTemplate(int id, UpdateTemplateDto dto, ApplicationDbContext db)
    {
        var template = await db.Templates.FindAsync(id);
        if (template is null)
            return Results.NotFound();

        template.Name = dto.Name;
        template.Description = dto.Description;
        template.CssClass = dto.CssClass;
        template.BackgroundColor = dto.BackgroundColor;
        template.TextColor = dto.TextColor;
        template.FontFamily = dto.FontFamily;
        template.IsActive = dto.IsActive;

        await db.SaveChangesAsync();

        return Results.Ok(MapToDto(template));
    }

    private static async Task<IResult> DeleteTemplate(int id, ApplicationDbContext db)
    {
        var template = await db.Templates.FindAsync(id);
        if (template is null)
            return Results.NotFound();

        db.Templates.Remove(template);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static TemplateDto MapToDto(Template template)
    {
        return new TemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            CssClass = template.CssClass,
            BackgroundColor = template.BackgroundColor,
            TextColor = template.TextColor,
            FontFamily = template.FontFamily,
            IsActive = template.IsActive,
            CreatedAt = template.CreatedAt
        };
    }
}
