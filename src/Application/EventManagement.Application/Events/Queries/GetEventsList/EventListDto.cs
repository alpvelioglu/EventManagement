using EventManagement.Application.Common.Mappings;
using EventManagement.Core.Entities;

namespace EventManagement.Application.Events.Queries.GetEventsList;

public class EventListDto : IMapFrom<Event>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CurrentParticipants { get; set; }
    public int MaxParticipants { get; set; }
}
