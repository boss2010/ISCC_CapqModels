using System.Diagnostics;

namespace Capqwebsite.Infrastructure;

public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var started = Stopwatch.GetTimestamp();
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        try
        {
            await next(context);

            if (context.Response.StatusCode >= 400)
            {
                logger.LogWarning(
                    "HTTP {StatusCode} {Method} {Path}{QueryString}; UserId={UserId}; EmployeeId={EmployeeId}; TraceId={TraceId}; DurationMs={DurationMs}",
                    context.Response.StatusCode,
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString,
                    context.Session.GetString("UserId") ?? "anonymous",
                    context.Session.GetString("EmployeeId") ?? "unknown",
                    traceId,
                    Stopwatch.GetElapsedTime(started).TotalMilliseconds);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                "Unhandled error in {Method} {Path}{QueryString}; UserId={UserId}; EmployeeId={EmployeeId}; TraceId={TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Session.GetString("UserId") ?? "anonymous",
                context.Session.GetString("EmployeeId") ?? "unknown",
                traceId);

            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync($"حدث خطأ غير متوقع. رقم تتبع المشكلة: {traceId}");
            }
        }
    }
}
