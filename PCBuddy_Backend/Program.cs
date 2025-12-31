using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Services;
using System.Text;
using System.Text.Json.Serialization;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<SyncService>();
builder.Services.AddScoped<ComputerService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpClient<AIService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JWT_KEY"] ?? throw new Exception("JWT_KEY missing in .env"))),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT_ISSUER"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT_AUDIENCE"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
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

app.MapGet("/", () => "Welcome to PCBuddy API!");

app.Run();