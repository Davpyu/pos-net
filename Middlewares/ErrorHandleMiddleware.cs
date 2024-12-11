using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pos.Exceptions;

namespace Pos.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly JsonSerializerOptions options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            string errorCode = "";
            response.ContentType = "application/json";

            switch (error)
            {
                case AppException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorCode = "not_found";
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Console.WriteLine(error?.Message);

            // await writeLog(context, dataContext, response.StatusCode, error?.Message);

            var result = JsonSerializer.Serialize(new { message = error?.Message, code = errorCode }, options);
            await response.WriteAsync(result);
        }
    }
}