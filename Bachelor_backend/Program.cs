
//https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0 accessed 18.01.2023. Used to set up CORS
using Bachelor_backend.DAL;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.DBInitializer;
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
builder.Services.AddScoped<ITextRepository, TextRepository>();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("JohanDesktop")));
builder.Services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
builder.Services.AddScoped<DBInitializer, DBInitializer>();
var app = builder.Build();
void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetService<DBInitializer>();
        dbInitializer.Initialize();
    }
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
SeedDatabase();

app.Run();
