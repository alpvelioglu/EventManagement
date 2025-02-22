using EventManagement.Core.Common;
using EventManagement.Core.Enums;

namespace EventManagement.Core.Entities;

public class Participant : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public ParticipationStatus Status { get; set; }
    
    public int EventId { get; set; }
    public virtual Event Event { get; set; } = default!;
}
