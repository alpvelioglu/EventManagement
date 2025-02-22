using EventManagement.Application.Events.Commands.CreateEvent;
using EventManagement.Application.Events.Queries.GetEventDetails;
using EventManagement.Application.Events.Queries.GetEventsList;
using EventManagement.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ISender _mediator;

    public EventsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<EventListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<EventListDto>>> GetEvents()
    {
        var result = await _mediator.Send(new GetEventsListQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EventDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDetailsDto>> GetEventDetails(int id)
    {
        var result = await _mediator.Send(new GetEventDetailsQuery(id));
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateEvent(CreateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? CreatedAtAction(nameof(GetEventDetails), new { id = result.Value }, result.Value) : BadRequest(result.Error);
    }
}
