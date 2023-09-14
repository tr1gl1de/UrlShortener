using System.Text.Json;

namespace UrlShortener.Models;

public readonly record struct ErrorDetails
{
    public int StatusCode { get; init; }
    public string Message { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}