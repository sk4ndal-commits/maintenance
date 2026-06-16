using MaintenanceSystem.Application.Common.Interfaces;

namespace MaintenanceSystem.API.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuditLogger auditLogger)
    {
        var method = context.Request.Method;
        if (method != "GET" && method != "OPTIONS" && method != "HEAD")
        {
            await auditLogger.LogAsync($"HttpRequest: {method} {context.Request.Path}", 
                $"User: {context.User?.Identity?.Name ?? "Anonymous"}");
        }
        await _next(context);
    }
}
