using Br.OrderManagement.API.Middlewares;
using Br.OrderManagement.CrossCutting.IoC;
using Br.OrderManagement.Repository.Persistence;
using Microsoft.EntityFrameworkCore;

var baseCorsPolicy = "_basePolicy";

var builder = WebApplication.CreateBuilder(args);

// =======================================
// Configuration
// =======================================

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// =======================================
// Services
// =======================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.RegisterAllClasses(builder.Configuration);

// CORS
builder.Services.AddCors(opt => {
    opt.AddPolicy(name: baseCorsPolicy, policy => {
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.WithOrigins(["http://localhost:4200", "http://localhost:*", "https://localhost/*", "http://localhost:61748"]);
    });
});

var app = builder.Build();

// =======================================
// Database
// =======================================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.Migrate();
}

// =======================================
// Middleware
// =======================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(baseCorsPolicy);

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();