using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Queries.GetEventsList;

public record GetEventsListQuery : IRequest<Result<IEnumerable<EventListDto>>>;
