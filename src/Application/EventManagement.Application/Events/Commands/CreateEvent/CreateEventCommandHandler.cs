using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Enums;
using EventManagement.Core.Interfaces;
using MediatR;

namespace EventManagement.Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<int>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IErrorLogger _errorLogger;

    public CreateEventCommandHandler(
        IRepository<Event> eventRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IErrorLogger errorLogger)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _errorLogger = errorLogger;
    }

    public async Task<Result<int>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = _mapper.Map<Event>(request);
            @event.Status = EventStatus.Draft;
            @event.CreatedAt = DateTime.UtcNow;

            var createdEvent = await _eventRepository.AddAsync(@event);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(createdEvent.Id);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<int>.Failure(ex.Message);
        }
    }
}
