using EventManagement.Application.Events.Commands.CancelEvent;
using EventManagement.Application.Events.Commands.CreateEvent;
using EventManagement.Application.Events.Commands.DeleteEvent;
using EventManagement.Application.Events.Commands.RegisterParticipant;
using EventManagement.Application.Events.Commands.RemoveParticipant;
using EventManagement.Application.Events.Commands.UpdateEvent;
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
    [ProducesResponseType(typeof(IEnumerable<EventListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EventListDto>>> GetEvents()
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

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> UpdateEvent(int id, UpdateEventCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteEvent(int id)
    {
        var result = await _mediator.Send(new DeleteEventCommand(id));
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> CancelEvent(int id)
    {
        var result = await _mediator.Send(new CancelEventCommand(id));
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("{id}/participants")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> RegisterParticipant(int id, [FromBody] int participantId)
    {
        var result = await _mediator.Send(new RegisterParticipantCommand(id, participantId));
        if (!result.IsSuccess)
        {
            return result.Error.Contains("not found") ? NotFound(result.Error) : BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}/participants/{participantId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> RemoveParticipant(int id, int participantId)
    {
        var result = await _mediator.Send(new RemoveParticipantCommand(id, participantId));
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
