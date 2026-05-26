using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Responses;

namespace Ludo.Infrastructure.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = MediaTypeNames.Application.Json;
            var responseError = ex is ServerException serverException ? ErrorMessage.FromException(serverException) : ErrorMessage.FromException(ex);
            response.StatusCode = (int) responseError.Status;
            await response.WriteAsync(JsonSerializer.Serialize(RequestResponse.FromError(responseError.LogError(logger))));
        }
    }
}
