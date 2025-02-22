using EventManagement.Application.Events.Commands.CancelEvent;
using EventManagement.Application.Events.Commands.RegisterParticipant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cancels an event
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelEvent(int id)
    {
        var result = await _mediator.Send(new CancelEventCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Registers a participant for an event
    /// </summary>
    /// <param name="command">Registration details</param>
    /// <returns>Success status</returns>
    [HttpPost("register-participant")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterParticipant([FromBody] RegisterParticipantCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
