using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Commands.UpdateEvent;

public record UpdateEventCommand : IRequest<Result<bool>>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int MaxParticipants { get; init; }
}
