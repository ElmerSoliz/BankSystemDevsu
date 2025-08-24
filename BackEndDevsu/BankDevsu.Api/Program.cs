using BankDevsu.Api.Middleware;
using BankDevsu.Api.Validation;
using BankDevsu.Application.Abstractions;
using BankDevsu.Application.Services;
using BankDevsu.Infrastructure.Persistence;
using BankDevsu.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

var services = builder.Services;
var config = builder.Configuration;

var conn = Environment.GetEnvironmentVariable("DB_CONN")
           ?? config.GetConnectionString("Default")
           ?? "Server=sqlserver,1433;Database=BankingDb;User Id=sa;Password=BankDevsu123;TrustServerCertificate=True;";

services.AddDbContext<BankingDbContext>(opt => opt.UseSqlServer(conn));

services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IAccountRepository, AccountRepository>();
services.AddScoped<IMovementRepository, MovementRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IMovementService, MovementService>();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<ClientCreateValidator>();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BankDevsu API",
        Version = "v1",
        Description = "API para gestión bancaria (Evaluación Devsu)",
        Contact = new OpenApiContact
        {
            Name = "Equipo Devsu",
            Email = "support@devsu.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankDevsu API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
