using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Commands.CreateEvent;

public record CreateEventCommand : IRequest<Result<int>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int MaxParticipants { get; init; }
    public int OrganizerId { get; init; }
    public int VenueId { get; init; }
}
