using EventManagement.API.Controllers;
using EventManagement.Application.Events.Commands.CancelEvent;
using EventManagement.Application.Events.Commands.RegisterParticipant;
using EventManagement.Core.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EventManagement.Tests.Controllers;

[TestFixture]
public class EventControllerTests
{
    private Mock<IMediator> _mockMediator;
    private EventController _controller;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new EventController(_mockMediator.Object);
    }

    [Test]
    public async Task CancelEvent_WhenSuccessful_ReturnsOkResult()
    {
        // Arrange
        var eventId = 1;
        _mockMediator.Setup(x => x.Send(It.IsAny<CancelEventCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _controller.CancelEvent(eventId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be(true);
    }

    [Test]
    public async Task CancelEvent_WhenFailed_ReturnsBadRequest()
    {
        // Arrange
        var eventId = 1;
        var errorMessage = "Failed to cancel event";
        _mockMediator.Setup(x => x.Send(It.IsAny<CancelEventCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Failure(errorMessage));

        // Act
        var result = await _controller.CancelEvent(eventId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().Be(errorMessage);
    }

    [Test]
    public async Task RegisterParticipant_WhenSuccessful_ReturnsOkResult()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _controller.RegisterParticipant(command);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be(true);
    }

    [Test]
    public async Task RegisterParticipant_WhenFailed_ReturnsBadRequest()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        var errorMessage = "Failed to register participant";
        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Failure(errorMessage));

        // Act
        var result = await _controller.RegisterParticipant(command);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().Be(errorMessage);
    }
}
