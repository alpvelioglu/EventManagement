using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using MediatR;

namespace EventManagement.Application.Events.Commands.RegisterParticipant;

public class RegisterParticipantCommandHandler : IRequestHandler<RegisterParticipantCommand, Result<bool>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorLogger _errorLogger;
    private readonly IMapper _mapper;

    public RegisterParticipantCommandHandler(
        IRepository<Event> eventRepository,
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IErrorLogger errorLogger,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _errorLogger = errorLogger;
        _mapper = mapper;
    }

    public async Task<Result<bool>> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event == null)
            {
                return Result<bool>.Failure("Event not found");
            }

            var participant = await _userRepository.GetByIdAsync(request.ParticipantId, cancellationToken);
            if (participant == null)
            {
                return Result<bool>.Failure("Participant not found");
            }

            if (@event.MaxParticipants > 0 && @event.RegisteredUsers.Count >= @event.MaxParticipants)
            {
                return Result<bool>.Failure("Event has reached maximum participants");
            }

            if (@event.RegisteredUsers.Any(u => u.Id == request.ParticipantId))
            {
                return Result<bool>.Failure("User is already registered for this event");
            }

            @event.RegisteredUsers.Add(participant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<bool>.Failure("Failed to register participant for event");
        }
    }
}
