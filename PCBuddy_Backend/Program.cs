using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Services;
using System.Text;
using System.Text.Json.Serialization;

// 1. Load Environment Variables
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// 2. Add Services
// Support MVC Views and API Controllers with Enum string conversion
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Custom Application Services
builder.Services.AddSingleton<SyncService>();
builder.Services.AddScoped<ComputerService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpClient<AIService>();

// 3. Authentication Configuration (Hybrid: JWT + Cookies)
builder.Services.AddAuthentication(options =>
{
    // Define a custom default scheme that routes to the policy
    options.DefaultScheme = "JWT_OR_COOKIE";
    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
})
.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
{
    // Dynamic Selector:
    // If URL starts with /api -> Use JWT
    // All other URLs -> Use Cookie
    options.ForwardDefaultSelector = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
        {
            return JwtBearerDefaults.AuthenticationScheme;
        }
        return CookieAuthenticationDefaults.AuthenticationScheme;
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.AccessDeniedPath = "/Admin/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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

// 4. Seeding Logic
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

// 5. Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Required for MVC styles/scripts

app.UseAuthentication();
app.UseAuthorization();

// Map API Controllers
app.MapControllers();

// Map MVC Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 6. DB Connection Check
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

app.Run();