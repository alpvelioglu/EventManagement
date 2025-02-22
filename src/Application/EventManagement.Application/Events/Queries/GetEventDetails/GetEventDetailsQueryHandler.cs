using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Application.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryHandler : IRequestHandler<GetEventDetailsQuery, Result<EventDetailsDto>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IErrorLogger _errorLogger;
    private readonly IMapper _mapper;

    public GetEventDetailsQueryHandler(
        IRepository<Event> eventRepository,
        IErrorLogger errorLogger,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _errorLogger = errorLogger;
        _mapper = mapper;
    }

    public async Task<Result<EventDetailsDto>> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var eventEntity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);
            if (eventEntity == null)
            {
                return Result<EventDetailsDto>.Failure("Event not found");
            }

            var eventDto = _mapper.Map<EventDetailsDto>(eventEntity);
            return Result<EventDetailsDto>.Success(eventDto);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<EventDetailsDto>.Failure("Failed to retrieve event details");
        }
    }
}
