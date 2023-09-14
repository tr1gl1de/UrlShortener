namespace UrlShortener.Services;

public class UrlShorteningService
{
    public const int LenghtOfCodeShortLink = 7;
    private const string Alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
    private readonly ApplicationDbContext _dbContext;

    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string GenerateUniqueCode()
    {
        while (true)
        {
            var codeChars = new char[LenghtOfCodeShortLink];
            for (var i = 0; i < LenghtOfCodeShortLink; i++)
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