using EventManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventManagement.Infrastructure.Services;

public class ErrorLogger : IErrorLogger
{
    private readonly ILogger<ErrorLogger> _logger;

    public ErrorLogger(ILogger<ErrorLogger> logger)
    {
        _logger = logger;
    }

    public async Task LogErrorAsync(Exception exception, int? statusCode = null)
    {
        if (statusCode.HasValue)
        {
            _logger.LogError(exception, "An error occurred with status code {StatusCode}: {ErrorMessage}", 
                statusCode.Value, exception.Message);
        }
        else
        {
            _logger.LogError(exception, "An error occurred: {ErrorMessage}", exception.Message);
        }

        await Task.CompletedTask;
    }
}
