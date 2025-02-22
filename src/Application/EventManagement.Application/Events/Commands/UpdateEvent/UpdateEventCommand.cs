using EventManagement.Application.Common.Mappings;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;
using MediatR;
using AutoMapper;

namespace EventManagement.Application.Events.Commands.UpdateEvent;

public record UpdateEventCommand : IRequest<Result<bool>>, IMapFrom<Event>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int MaxParticipants { get; init; }
    public int OrganizerId { get; init; }
    public int VenueId { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateEventCommand, Event>()
            .ForMember(d => d.Status, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.IsDeleted, opt => opt.Ignore())
            .ForMember(d => d.CurrentParticipants, opt => opt.Ignore())
            .ForMember(d => d.Organizer, opt => opt.Ignore())
            .ForMember(d => d.Venue, opt => opt.Ignore())
            .ForMember(d => d.Participants, opt => opt.Ignore())
            .ForMember(d => d.RegisteredUsers, opt => opt.Ignore());

        profile.CreateMap<Event, UpdateEventCommand>();
    }
}
