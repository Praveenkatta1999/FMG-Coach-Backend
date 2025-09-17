using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Scrutor;

namespace FMG_Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<CoachService>();
        builder.Services.AddScoped<TeamService>();
        builder.Services.AddScoped<GameService>();

        // inside your ConfigureServices or builder.Services setup
        // var assembly = Assembly.GetExecutingAssembly(); // or specify your Services assembly if separate

        // // Register all classes that end with "Service" as scoped services
        // builder.Services.Scan(scan => scan
        //     .FromAssemblies(assembly)
        //     .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
        //     .AsImplementedInterfaces()  // registers by the interfaces they implement
        //     .WithScopedLifetime());

        builder.Services.AddControllers();

        builder.Services.AddDbContext<GameDbContext>(options =>
            options.UseSqlite("Data Source=Database/FMGdatabase_Test.db")
        );

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Warning: Failed to determine the https port for redirect.
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
