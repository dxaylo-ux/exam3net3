using System.Diagnostics;

namespace WebAPI.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context);

        timer.Stop();
        _logger.LogInformation("Response StatusCode: {StatusCode}", context.Response.StatusCode);
        _logger.LogInformation("Execution time: {Elapsed}ms", timer.ElapsedMilliseconds);
    }
}
