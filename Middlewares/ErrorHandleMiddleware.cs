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
                    if (e.Code.Contains("not_found"))
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                    }

                    else
                    {
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }

                    errorCode = e.Code;
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