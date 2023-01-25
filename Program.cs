
//https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0 accessed 18.01.2023. Used to set up CORS
using Bachelor_backend.DAL;
using Bachelor_backend.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:3000");
        });
});

// Initialized logger (code accessed from https://www.claudiobernasconi.ch/2022/01/28/how-to-use-serilog-in-asp-net-core-web-api/) 
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddScoped<IVoiceRepository, VoiceRepository>();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("JohanDesktop")));

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
