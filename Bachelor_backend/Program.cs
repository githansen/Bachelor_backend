
//https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0 accessed 18.01.2023. Used to set up CORS
using Bachelor_backend.DAL;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.DBInitializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyMethod()
            .AllowAnyHeader().WithOrigins("https://gidinstemme-front-test.azurewebsites.net/");
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
builder.Services.AddTransient<IAzureStorage, AzureStorage>();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

// Configuring sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(1800); //30 Minutes session
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<DBInitializer, DBInitializer>();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(1800); //30 Minutes session
    options.Cookie.IsEssential = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
 $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API GiDinStemme", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});

var app = builder.Build();
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetService<DBInitializer>();
        dbInitializer.Initialize();
    }
}


app.UseSession();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
SeedDatabase();

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "GiDinStemme API"); //originally "./swagger/v1/swagger.json"
});
app.Run();
