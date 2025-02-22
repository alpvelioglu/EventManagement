using AutoMapper;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using EventManagement.Core.Exceptions;
using EventManagement.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Result<bool>>
{
    private readonly IRepository<Event> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorLogger _errorLogger;
    private readonly IMapper _mapper;

    public DeleteEventCommandHandler(
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

    public async Task<Result<bool>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await _eventRepository.Query()
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (@event == null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }

            if (@event.Participants.Any())
            {
                return Result<bool>.Failure("Cannot delete an event with registered participants");
            }

            await _eventRepository.DeleteAsync(@event);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogErrorAsync(ex);
            return Result<bool>.Failure(ex.Message);
        }
    }
}
