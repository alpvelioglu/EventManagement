using EventManagement.Core.Common;
using EventManagement.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Core.Entities;

public class Event : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public EventStatus Status { get; set; }
    
    public int OrganizerId { get; set; }
    public virtual Organizer Organizer { get; set; } = null!;
    
    public int VenueId { get; set; }
    public virtual Venue Venue { get; set; } = null!;
    
    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
    public virtual ICollection<User> RegisteredUsers { get; set; } = new List<User>();
}
