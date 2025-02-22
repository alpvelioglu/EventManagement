using AutoMapper;
using EventManagement.Application.Common.Mappings;
using EventManagement.Core.Entities;
using EventManagement.Core.Enums;

namespace EventManagement.Application.Events.Queries.GetEventDetails;

public class EventDetailsDto : IMapFrom<Event>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public OrganizerDto Organizer { get; set; } = null!;
    public VenueDto Venue { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Event, EventDetailsDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.CurrentParticipants, opt => opt.MapFrom(s => s.Participants.Count));
    }
}

public class OrganizerDto : IMapFrom<Organizer>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class VenueDto : IMapFrom<Venue>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Capacity { get; set; }
}
