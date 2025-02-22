namespace EventManagement.Core.Interfaces;

public interface IErrorLogger
{
    Task LogErrorAsync(Exception exception, int? statusCode = null);
}
