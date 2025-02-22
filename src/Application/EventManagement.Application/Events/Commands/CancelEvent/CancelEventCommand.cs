using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Commands.CancelEvent;

public record CancelEventCommand(int Id) : IRequest<Result<bool>>;
