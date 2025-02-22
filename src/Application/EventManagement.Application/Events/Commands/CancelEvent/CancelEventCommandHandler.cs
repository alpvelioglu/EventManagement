using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using EventManagement.Core.Enums;
using MediatR;

namespace EventManagement.Application.Events.Commands.CancelEvent;

public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, Result<bool>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorLogger _errorLogger;
    private readonly IMapper _mapper;

    public CancelEventCommandHandler(
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

    public async Task<Result<bool>> Handle(CancelEventCommand request, CancellationToken cancellationToken)
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
                return Result<bool>.Failure("Event is already cancelled");
            }

            if (@event.StartDate < DateTime.UtcNow)
            {
                return Result<bool>.Failure("Cannot cancel a past event");
            }

            @event.Status = EventStatus.Cancelled;
            @event.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(@event, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<bool>.Failure("Failed to cancel event");
        }
    }
}
