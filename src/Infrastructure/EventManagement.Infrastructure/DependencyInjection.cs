using Azure.Identity;
using EventManagement.Core.Interfaces;
using EventManagement.Infrastructure.Persistence;
using EventManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EventManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => 
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure();
                }));

        // Register repositories and services
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IErrorLogger, DatabaseErrorLogger>();
        services.AddScoped<ApplicationDbContextInitializer>();

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        // Configure Azure Key Vault if enabled
        if (configuration.GetValue<bool>("UseAzureKeyVault"))
        {
            var keyVaultUrl = configuration["KeyVault:Url"];
            if (!string.IsNullOrEmpty(keyVaultUrl))
            {
                var credential = new DefaultAzureCredential();
                services.AddSingleton(credential);
            }
        }

        return services;
    }
}
