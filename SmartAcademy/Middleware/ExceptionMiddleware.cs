using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case BadRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(new { message = exception.Message });
        return context.Response.WriteAsync(result);
    }
}