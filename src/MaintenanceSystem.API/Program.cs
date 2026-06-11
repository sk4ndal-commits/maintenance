using MaintenanceSystem.Application.Assets.Commands;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Infrastructure.Persistence;
using MaintenanceSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=maintenance.db"));

builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<CreateAssetHandler>();
builder.Services.AddScoped<UpdateAssetHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("VueFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
