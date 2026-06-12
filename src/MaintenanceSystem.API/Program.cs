using MaintenanceSystem.Application.Assets.Commands;
using MaintenanceSystem.Application.Assets.Queries;
using MaintenanceSystem.Application.WorkOrders.Commands;
using MaintenanceSystem.Application.Users.Commands;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Infrastructure.Persistence;
using MaintenanceSystem.Infrastructure.Services;
using MaintenanceSystem.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=maintenance.db"));

builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<CreateAssetHandler>();
builder.Services.AddScoped<UpdateAssetHandler>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();
builder.Services.AddScoped<CreateWorkOrderHandler>();
builder.Services.AddScoped<AssignWorkOrderHandler>();
builder.Services.AddScoped<ChangeWorkOrderStatusHandler>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<IAssignmentHistoryRepository, AssignmentHistoryRepository>();
builder.Services.AddScoped<IChecklistStepRepository, ChecklistStepRepository>();
builder.Services.AddScoped<AddChecklistStepHandler>();
builder.Services.AddScoped<CompleteChecklistStepHandler>();
builder.Services.AddScoped<CompleteWorkOrderHandler>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IMaintenanceDocumentRepository, MaintenanceDocumentRepository>();
builder.Services.AddScoped<UploadMaintenanceDocumentHandler>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<GetAssetHistoryHandler>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<SetUserActiveHandler>();
builder.Services.AddScoped<LoginHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
        policy.AllowAnyOrigin()
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
