using ChatApplication.Shared.Exceptions.Auth;
using Newtonsoft.Json;

namespace ChatApplication.API.Middleware;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = exception switch
        {
            AuthException _ => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
            
        return context.Response.WriteAsync(JsonConvert.SerializeObject(new {message = exception.Message}));
    }
}