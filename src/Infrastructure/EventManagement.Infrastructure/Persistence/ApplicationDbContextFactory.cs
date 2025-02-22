using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventManagement.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var apiProjectPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "Presentation", "EventManagement.API"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        optionsBuilder.UseSqlServer(connectionString, 
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
            });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
