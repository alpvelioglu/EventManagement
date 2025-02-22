using EventManagement.Core.Common;
using MediatR;

namespace EventManagement.Application.Events.Commands.RegisterParticipant;

public record RegisterParticipantCommand(int EventId, int ParticipantId) : IRequest<Result<bool>>;
