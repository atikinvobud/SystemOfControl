using System.Text.Json;
using BackEnd.DTOs;
using BackEnd.Models;
using BackEnd.Repositories;
using BackEnd.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BackEnd.Extensions;

public static class ProgrammcsExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // настройка контроллеров
        services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = false;
            });

        // подключение к postgres
        var connectionString = Environment.GetEnvironmentVariable("DatabaseConnection");
        services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));

        // подключение Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SystemOfControl",
                Version = "v1"
            });
        });

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICoockieService, CoockieService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<AbstractValidator<RegistrDTO>, UserValidator>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddHttpContextAccessor();
        
        services.Configure<JwtSettings>( configuration.GetSection("JwtSettings"));
        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        // Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SystemOfControl v1");
            c.RoutePrefix = string.Empty; // Swagger будет доступен по адресу /
        });

        // автоматическое применение миграций
        app.ApplyMigrations();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }
}
