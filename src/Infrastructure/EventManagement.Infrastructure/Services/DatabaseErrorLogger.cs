using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventManagement.Infrastructure.Services;

public class DatabaseErrorLogger : IErrorLogger
{
    private readonly IRepository<ErrorLog> _errorLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DatabaseErrorLogger> _logger;

    public DatabaseErrorLogger(
        IRepository<ErrorLog> errorLogRepository,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        ILogger<DatabaseErrorLogger> logger)
    {
        _errorLogRepository = errorLogRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task LogErrorAsync(Exception exception, int? statusCode = null)
    {
        try
        {
            var context = _httpContextAccessor.HttpContext;
            var errorLog = new ErrorLog
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Source = exception.Source,
                RequestPath = context?.Request.Path.Value,
                RequestMethod = context?.Request.Method,
                UserAgent = context?.Request.Headers["User-Agent"].ToString(),
                IpAddress = context?.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow,
                ErrorType = exception.GetType().Name,
                StatusCode = statusCode,
                AdditionalInfo = exception.InnerException?.Message
            };

            await _errorLogRepository.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // If database logging fails, log to file using Serilog
            _logger.LogError(ex, "Failed to log error to database");
        }
    }
}
