
//https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0 accessed 18.01.2023. Used to set up CORS
using Microsoft.EntityFrameworkCore;
using Bachelor_backend.DAL;

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

builder.Services.AddControllers();
builder.Services.AddScoped<IVoiceRepository, VoiceRepository>();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("JohanLaptop")));

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
