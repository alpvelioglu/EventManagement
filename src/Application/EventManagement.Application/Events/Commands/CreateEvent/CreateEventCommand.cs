using EventManagement.Application.Common.Mappings;
using EventManagement.Core.Entities;
using EventManagement.Core.Common;
using MediatR;
using AutoMapper;

namespace EventManagement.Application.Events.Commands.CreateEvent;

public record CreateEventCommand : IRequest<Result<int>>, IMapFrom<Event>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int MaxParticipants { get; init; }
    public int OrganizerId { get; init; }
    public int VenueId { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateEventCommand, Event>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Status, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.IsDeleted, opt => opt.Ignore())
            .ForMember(d => d.CurrentParticipants, opt => opt.Ignore())
            .ForMember(d => d.Organizer, opt => opt.Ignore())
            .ForMember(d => d.Venue, opt => opt.Ignore())
            .ForMember(d => d.Participants, opt => opt.Ignore())
            .ForMember(d => d.RegisteredUsers, opt => opt.Ignore());
    }
}
