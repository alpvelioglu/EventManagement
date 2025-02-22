using FluentValidation;

namespace EventManagement.Application.Events.Commands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(v => v.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow);

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .GreaterThan(v => v.StartDate);

        RuleFor(v => v.MaxParticipants)
            .GreaterThan(0);

        RuleFor(v => v.OrganizerId)
            .GreaterThan(0);

        RuleFor(v => v.VenueId)
            .GreaterThan(0);
    }
}
