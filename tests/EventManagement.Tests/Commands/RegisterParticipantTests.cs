using EventManagement.Application.Events.Commands.RegisterParticipant;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EventManagement.Tests.Commands;

[TestFixture]
public class RegisterParticipantTests : TestBase
{
    private Mock<IRepository<Event>> _mockEventRepository;
    private Mock<IRepository<User>> _mockUserRepository;
    private RegisterParticipantCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _mockEventRepository = new Mock<IRepository<Event>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _handler = new RegisterParticipantCommandHandler(
            _mockEventRepository.Object,
            _mockUserRepository.Object,
            MockUnitOfWork.Object,
            MockErrorLogger.Object,
            MockMapper.Object);
    }

    [Test]
    public async Task Handle_WhenEventDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Event not found");
    }

    [Test]
    public async Task Handle_WhenParticipantDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        var existingEvent = new Event { Id = 1 };
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);
        _mockUserRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Participant not found");
    }

    [Test]
    public async Task Handle_WhenEventIsFull_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        var existingEvent = new Event 
        { 
            Id = 1,
            MaxParticipants = 1,
            RegisteredUsers = new List<User> { new User { Id = 2 } }
        };
        var participant = new User { Id = 1 };

        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);
        _mockUserRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(participant);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Event has reached maximum participants");
    }

    [Test]
    public async Task Handle_WhenValidRegistration_ShouldRegisterParticipantAndReturnSuccess()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        var existingEvent = new Event 
        { 
            Id = 1,
            MaxParticipants = 2,
            RegisteredUsers = new List<User>()
        };
        var participant = new User { Id = 1 };

        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);
        _mockUserRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(participant);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        existingEvent.RegisteredUsers.Should().Contain(participant);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenExceptionOccurs_ShouldLogErrorAndReturnFailure()
    {
        // Arrange
        var command = new RegisterParticipantCommand(1, 1);
        var exception = new Exception("Test exception");
        _mockEventRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to register participant");
        MockErrorLogger.Verify(x => x.LogErrorAsync(exception), Times.Once);
    }
}
