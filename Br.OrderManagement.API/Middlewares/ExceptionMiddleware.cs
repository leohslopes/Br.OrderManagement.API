using Br.OrderManagement.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Br.OrderManagement.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success = false,
                message = ex.Message
            }));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success = false,
                message = ex.Message
            }));
        }
    }
}

