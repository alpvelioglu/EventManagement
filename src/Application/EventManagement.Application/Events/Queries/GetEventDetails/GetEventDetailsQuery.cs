using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Queries.GetEventDetails;

public record GetEventDetailsQuery(int Id) : IRequest<Result<EventDetailsDto>>;
