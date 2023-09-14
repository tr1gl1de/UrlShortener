namespace UrlShortener.Entities;

public class ShortenedUrl
{
    public Guid Id { get; init; }
    public string LongUrl { get; init; } = string.Empty;
    public string ShortUrl { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
}