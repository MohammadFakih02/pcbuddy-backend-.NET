using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<SyncService>();
builder.Services.AddScoped<ComputerService>();

var app = builder.Build();

if (args.Length > 0 && args[0].ToLower() == "seed")
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            Console.WriteLine(" Starting Database Seeder...");

            DataSeeder.Seed(context);

            Console.WriteLine(" Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error during seeding: {ex.Message}");
        }
    }
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        if (context.Database.CanConnect())
        {
            Console.WriteLine(" Database connection successful!");
        }
        else
        {
            Console.WriteLine(" Database connected, but might not be created yet (Run Migrations).");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Database connection failed: {ex.Message}");
    }
}
app.MapGet("/", () => "Welcome to PCBuddy API! Use /api/computer/... or /api/sync/...");

app.Run();
