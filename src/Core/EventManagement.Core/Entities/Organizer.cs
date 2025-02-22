using EventManagement.Core.Common;

namespace EventManagement.Core.Entities;

public class Organizer : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
