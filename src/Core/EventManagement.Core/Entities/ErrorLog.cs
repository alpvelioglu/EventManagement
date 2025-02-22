using EventManagement.Core.Common;

namespace EventManagement.Core.Entities;

public class ErrorLog : BaseEntity
{
    public string Message { get; set; } = default!;
    public string? StackTrace { get; set; }
    public string? Source { get; set; }
    public string? RequestPath { get; set; }
    public string? RequestMethod { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public DateTime Timestamp { get; set; }
    public string ErrorType { get; set; } = default!;
    public string? AdditionalInfo { get; set; }
    public int? StatusCode { get; set; }
}
