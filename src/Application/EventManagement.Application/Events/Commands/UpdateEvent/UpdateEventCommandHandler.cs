using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using EventManagement.Core.Enums;
using MediatR;

namespace EventManagement.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, Result<bool>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorLogger _errorLogger;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(
        IRepository<Event> eventRepository,
        IUnitOfWork unitOfWork,
        IErrorLogger errorLogger,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _errorLogger = errorLogger;
        _mapper = mapper;
    }

    public async Task<Result<bool>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

            if (@event == null)
            {
                return Result<bool>.Failure("Event not found");
            }

            if (@event.Status == EventStatus.Cancelled)
            {
                return Result<bool>.Failure("Cannot update a cancelled event");
            }

            if (request.MaxParticipants < @event.CurrentParticipants)
            {
                return Result<bool>.Failure("Cannot reduce max participants below current participant count");
            }

            if (request.StartDate >= request.EndDate)
            {
                return Result<bool>.Failure("End date must be after start date");
            }

            _mapper.Map(request, @event);
            @event.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(@event, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<bool>.Failure("Failed to update event");
        }
    }
}
