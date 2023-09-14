namespace UrlShortener.Models;

public readonly record struct ShortenUrlRequest()
{
    public string Url { get; init; } = string.Empty;
}