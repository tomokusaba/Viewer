using Microsoft.EntityFrameworkCore;
using DigitalSignage.Shared.Models;

namespace DigitalSignage.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Content> Contents => Set<Content>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<ContentTag> ContentTags => Set<ContentTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Content entity
        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.Url).HasMaxLength(500);
            entity.HasOne(e => e.Template)
                .WithMany(t => t.Contents)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Tag entity
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Template entity
        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CssClass).HasMaxLength(200);
            entity.Property(e => e.BackgroundColor).HasMaxLength(50);
            entity.Property(e => e.TextColor).HasMaxLength(50);
            entity.Property(e => e.FontFamily).HasMaxLength(100);
        });

        // Configure ContentTag join entity
        modelBuilder.Entity<ContentTag>(entity =>
        {
            entity.HasKey(ct => new { ct.ContentId, ct.TagId });
            entity.HasOne(ct => ct.Content)
                .WithMany(c => c.ContentTags)
                .HasForeignKey(ct => ct.ContentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ct => ct.Tag)
                .WithMany(t => t.ContentTags)
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed default templates
        modelBuilder.Entity<Template>().HasData(
            new Template
            {
                Id = 1,
                Name = "Default",
                Description = "Default template",
                CssClass = "template-default",
                BackgroundColor = "#ffffff",
                TextColor = "#000000",
                IsActive = true
            },
            new Template
            {
                Id = 2,
                Name = "Dark",
                Description = "Dark theme template",
                CssClass = "template-dark",
                BackgroundColor = "#1a1a2e",
                TextColor = "#eaeaea",
                IsActive = true
            },
            new Template
            {
                Id = 3,
                Name = "Vibrant",
                Description = "Colorful vibrant template",
                CssClass = "template-vibrant",
                BackgroundColor = "#667eea",
                TextColor = "#ffffff",
                IsActive = true
            }
        );
    }
}
