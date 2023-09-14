using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Entities;
using UrlShortener.Extensions;
using UrlShortener.Middlewares;
using UrlShortener.Models;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("application.json", true, true);
builder.Configuration.AddJsonFile($"application.{builder.Environment.EnvironmentName}.json", true, true);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => 
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.ApplyMigrations();
}

app.MapGet("/", async context =>
{
    context.Response.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/html; charset=UTF-8");
    await context.Response.SendFileAsync("wwwroot/index.html");
});

app.MapPost("api/shorten", async (ShortenUrlRequest req,
        UrlShorteningService urlShorteningService,
        ApplicationDbContext dbContext,
        HttpContext httpContext) =>
{
    if (!Uri.TryCreate(req.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified URL is invalid.");
    }

    var code = urlShorteningService.GenerateUniqueCode();
    var shortenedUrl = new ShortenedUrl
    {
        Id = Guid.NewGuid(),
        LongUrl = req.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{code}",
        CreatedOnUtc = DateTime.UtcNow
    };

    dbContext.ShortenedUrls.Add(shortenedUrl);
     await dbContext.SaveChangesAsync();

     return Results.Ok(shortenedUrl.ShortUrl);
});

app.MapGet("{code}", async (string code, ApplicationDbContext dbContext) =>
{
    var shortenedUrl = await dbContext.ShortenedUrls
        .FirstOrDefaultAsync(x => x.Code == code);

    return shortenedUrl is null 
        ? Results.NotFound() 
        : Results.Redirect(shortenedUrl.LongUrl);
});

app.UseHttpsRedirection();

app.Run();