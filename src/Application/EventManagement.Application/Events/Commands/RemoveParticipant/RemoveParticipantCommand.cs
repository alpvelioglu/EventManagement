using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Commands.RemoveParticipant;

public record RemoveParticipantCommand(int EventId, int ParticipantId) : IRequest<Result<bool>>;
