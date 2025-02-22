using EventManagement.Core.Common;

namespace EventManagement.Core.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public virtual ICollection<Event> RegisteredEvents { get; set; } = new List<Event>();
}
