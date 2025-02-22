using System.Text.Json;
using EventManagement.Application.Common.Exceptions;
using EventManagement.Core.Common;
using EventManagement.Core.Interfaces;
using FluentValidation;

namespace EventManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public ExceptionHandlingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var errorLogger = scope.ServiceProvider.GetRequiredService<IErrorLogger>();
            // Use errorLogger here
        }

        await _next(context);
    }
}
