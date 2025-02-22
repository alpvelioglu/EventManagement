using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Commands.DeleteEvent;

public record DeleteEventCommand(int Id) : IRequest<Result<bool>>;
