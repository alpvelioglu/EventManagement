using EventManagement.Core.Entities;
using EventManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventManagement.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Seed Organizers
        if (!await _context.Organizers.AnyAsync())
        {
            await _context.Organizers.AddRangeAsync(
                new Organizer { Name = "Tech Events Inc.", Email = "contact@techevents.com", PhoneNumber = "+1234567890" },
                new Organizer { Name = "Community Gatherings", Email = "info@communitygatherings.org", PhoneNumber = "+0987654321" }
            );
            await _context.SaveChangesAsync();
        }

        // Seed Venues
        if (!await _context.Venues.AnyAsync())
        {
            await _context.Venues.AddRangeAsync(
                new Venue { Name = "Tech Hub", Address = "123 Innovation St", Capacity = 500 },
                new Venue { Name = "Community Center", Address = "456 Main St", Capacity = 200 }
            );
            await _context.SaveChangesAsync();
        }

        // Seed Events
        if (!await _context.Events.AnyAsync())
        {
            var organizers = await _context.Organizers.ToListAsync();
            var venues = await _context.Venues.ToListAsync();

            await _context.Events.AddRangeAsync(
                new Event 
                { 
                    Name = "Tech Conference 2025",
                    Description = "Annual technology conference featuring the latest innovations",
                    StartDate = DateTime.UtcNow.AddMonths(1),
                    EndDate = DateTime.UtcNow.AddMonths(1).AddDays(2),
                    Status = EventStatus.Scheduled,
                    MaxParticipants = 400,
                    OrganizerId = organizers[0].Id,
                    VenueId = venues[0].Id
                },
                new Event 
                { 
                    Name = "Community Workshop",
                    Description = "Hands-on workshop for community development",
                    StartDate = DateTime.UtcNow.AddMonths(2),
                    EndDate = DateTime.UtcNow.AddMonths(2).AddDays(1),
                    Status = EventStatus.Draft,
                    MaxParticipants = 150,
                    OrganizerId = organizers[1].Id,
                    VenueId = venues[1].Id
                }
            );
            await _context.SaveChangesAsync();
        }
    }
}
