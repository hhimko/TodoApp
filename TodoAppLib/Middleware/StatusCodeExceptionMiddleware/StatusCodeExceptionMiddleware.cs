using Microsoft.AspNetCore.Http;

namespace TodoAppLib.Middleware.StatusCodeExceptionMiddleware;

public class StatusCodeExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public StatusCodeExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (StatusCodeException e) {
            context.Response.StatusCode = e.StatusCode;
            await context.Response.WriteAsync(e.Message);
        }
    }
}
