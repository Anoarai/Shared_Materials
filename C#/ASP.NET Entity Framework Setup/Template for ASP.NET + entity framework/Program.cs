using Microsoft.EntityFrameworkCore;
using Template_for_ASP.NET___entity_framework;
using Template_for_ASP.NET___entity_framework.Database;
using Template_for_ASP.NET___entity_framework.Interfaces;
using Template_for_ASP.NET___entity_framework.Services;

var builder = WebApplication.CreateBuilder(args);
var testapi = builder.Configuration["User:Username"];

// Add services to the container.
builder.Services.AddMvc();
ConfigureDb(builder.Services);
builder.Services.AddScoped<ILogService, LogService>();






var app = builder.Build();

app.MapControllers();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<LoggingMiddleware>();

app.Run();


static void ConfigureDb(IServiceCollection services)
{
    var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("Default");
    services.AddDbContext<ApplicationDbContext>(b => b.UseSqlServer(connectionString));
}


public partial class Program { }