using Microsoft.EntityFrameworkCore;
using UrlShortener.Entities;
using UrlShortener.Services;

namespace UrlShortener;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(x => x.Code).HasMaxLength(UrlShorteningService.LenghtOfCodeShortLink);

            builder.HasIndex(x => x.Code).IsUnique();
        });
    }
}