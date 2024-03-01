using Microsoft.Extensions.Options;
using UrlShortener.Models;

namespace UrlShortener.Services;

public class UrlShorteningService
{
    public readonly int LengthOfCodeShortLink;
    private const string Alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
    private readonly ApplicationDbContext _dbContext;

    public UrlShorteningService(ApplicationDbContext dbContext, IOptions<UrlShorteningServiceOptions> options)
    {
        _dbContext = dbContext;
        LengthOfCodeShortLink = options.Value.LengthOfCodeShortLink;
    }

    public string GetOrAddCode(string oldUrl)
    {
        var existCode = _dbContext.ShortenedUrls.FirstOrDefault(x => x.LongUrl == oldUrl);
        return existCode is null 
            ? GenerateUniqueCode() 
            : existCode.Code;
    }

    private string GenerateUniqueCode()
    {
        while (true)
        {
            var codeChars = new char[LengthOfCodeShortLink];
            for (var i = 0; i < LengthOfCodeShortLink; i++)
            {
                var randIndex = Random.Shared.Next(Alphabet.Length - 1);
                codeChars[i] = Alphabet[randIndex];
            }

            var code = new string(codeChars);

            if (!_dbContext.ShortenedUrls.Any(x => x.Code == code))
            {
                return code;
            }
        }
    }
}