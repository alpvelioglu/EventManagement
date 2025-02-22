using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Application.Events.Queries.GetEventsList;

public class GetEventsListQueryHandler : IRequestHandler<GetEventsListQuery, Result<IEnumerable<EventListDto>>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IErrorLogger _errorLogger;
    private readonly IMapper _mapper;

    public GetEventsListQueryHandler(
        IRepository<Event> eventRepository,
        IErrorLogger errorLogger,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _errorLogger = errorLogger;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<EventListDto>>> Handle(GetEventsListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var events = await _eventRepository.Query()
                .Include(e => e.Participants)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<IEnumerable<EventListDto>>(events);
            return Result<IEnumerable<EventListDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<IEnumerable<EventListDto>>.Failure("Failed to retrieve events");
        }
    }
}
