using Microsoft.EntityFrameworkCore;
using DigitalSignage.Server.Data;
using DigitalSignage.Shared.DTOs;
using DigitalSignage.Shared.Models;

namespace DigitalSignage.Server.Endpoints;

public static class TagEndpoints
{
    public static void MapTagEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tags").WithTags("Tags");

        group.MapGet("/", GetAllTags).WithName("GetAllTags");
        group.MapGet("/{id:int}", GetTagById).WithName("GetTagById");
        group.MapPost("/", CreateTag).WithName("CreateTag");
        group.MapPut("/{id:int}", UpdateTag).WithName("UpdateTag");
        group.MapDelete("/{id:int}", DeleteTag).WithName("DeleteTag");
    }

    private static async Task<IResult> GetAllTags(ApplicationDbContext db)
    {
        var tags = await db.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();

        var dtos = tags.Select(MapToDto).ToList();
        return Results.Ok(dtos);
    }

    private static async Task<IResult> GetTagById(int id, ApplicationDbContext db)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag is null)
            return Results.NotFound();

        return Results.Ok(MapToDto(tag));
    }

    private static async Task<IResult> CreateTag(CreateTagDto dto, ApplicationDbContext db)
    {
        var tag = new Tag
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        db.Tags.Add(tag);
        await db.SaveChangesAsync();

        return Results.Created($"/api/tags/{tag.Id}", MapToDto(tag));
    }

    private static async Task<IResult> UpdateTag(int id, UpdateTagDto dto, ApplicationDbContext db)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag is null)
            return Results.NotFound();

        tag.Name = dto.Name;
        tag.Description = dto.Description;

        await db.SaveChangesAsync();

        return Results.Ok(MapToDto(tag));
    }

    private static async Task<IResult> DeleteTag(int id, ApplicationDbContext db)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag is null)
            return Results.NotFound();

        db.Tags.Remove(tag);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static TagDto MapToDto(Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAt = tag.CreatedAt
        };
    }
}
