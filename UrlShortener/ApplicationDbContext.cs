using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Entities;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener;

public class ApplicationDbContext : DbContext
{
    private readonly UrlShorteningServiceOptions _options;
    
    public ApplicationDbContext(DbContextOptions options, IOptions<UrlShorteningServiceOptions> options1) : base(options)
    {
        _options = options1.Value;
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(x => x.Code).HasMaxLength(_options.LengthOfCodeShortLink);

            builder.HasIndex(x => x.Code).IsUnique();
        });
    }
}