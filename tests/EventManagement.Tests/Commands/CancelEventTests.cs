using EventManagement.Application.Events.Commands.CancelEvent;
using EventManagement.Core.Entities;
using EventManagement.Core.Enums;
using EventManagement.Core.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EventManagement.Tests.Commands;

[TestFixture]
public class CancelEventTests : TestBase
{
    private Mock<IRepository<Event>> _mockEventRepository;
    private CancelEventCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _mockEventRepository = new Mock<IRepository<Event>>();
        _handler = new CancelEventCommandHandler(
            _mockEventRepository.Object,
            MockUnitOfWork.Object,
            MockErrorLogger.Object,
            MockMapper.Object);
    }

    [Test]
    public async Task Handle_WhenEventDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new CancelEventCommand(1);
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Event not found");
    }

    [Test]
    public async Task Handle_WhenEventIsAlreadyCancelled_ShouldReturnFailure()
    {
        // Arrange
        var command = new CancelEventCommand(1);
        var existingEvent = new Event { Id = 1, Status = EventStatus.Cancelled };
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Event is already cancelled");
    }

    [Test]
    public async Task Handle_WhenEventIsPast_ShouldReturnFailure()
    {
        // Arrange
        var command = new CancelEventCommand(1);
        var existingEvent = new Event 
        { 
            Id = 1, 
            Status = EventStatus.Active,
            StartDate = DateTime.UtcNow.AddDays(-1)
        };
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Cannot cancel a past event");
    }

    [Test]
    public async Task Handle_WhenValidEvent_ShouldCancelEventAndReturnSuccess()
    {
        // Arrange
        var command = new CancelEventCommand(1);
        var existingEvent = new Event 
        { 
            Id = 1, 
            Status = EventStatus.Active,
            StartDate = DateTime.UtcNow.AddDays(1)
        };
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        existingEvent.Status.Should().Be(EventStatus.Cancelled);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenExceptionOccurs_ShouldLogErrorAndReturnFailure()
    {
        // Arrange
        var command = new CancelEventCommand(1);
        var exception = new Exception("Test exception");
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to cancel event");
        MockErrorLogger.Verify(x => x.LogErrorAsync(exception), Times.Once);
    }
}
