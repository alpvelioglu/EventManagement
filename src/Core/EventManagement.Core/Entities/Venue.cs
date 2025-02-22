using EventManagement.Core.Common;

namespace EventManagement.Core.Entities;

public class Venue : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public int Capacity { get; set; }
    public string? Description { get; set; }
    
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
