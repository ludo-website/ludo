using System.Diagnostics;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Ludo.Infrastructure.Errors;

public class ErrorMessage(HttpStatusCode status, string message, ErrorCodes code = ErrorCodes.Unknown, string? stackTrace = default)
{
    public string Message { get; } = message;
    public ErrorCodes Code { get; } = code;
    public HttpStatusCode Status { get; } = status;

    [JsonIgnore]
    public string StackTrace { get; } = stackTrace ?? new StackTrace(true).ToString();

    public static ErrorMessage FromException(ServerException exception) => new(exception.Status, exception.Message, stackTrace: exception.StackTrace);
    public static ErrorMessage FromException(Exception exception) => new(HttpStatusCode.InternalServerError, exception.Message, stackTrace: exception.StackTrace);

    public ErrorMessage LogError(ILogger? logger)
    {
        logger?.LogError("ErrorMessage {{ Status: {Status}, Code {Code}, Message: {Message} }}\r\n{StackTrace}", Status, Code, Message, StackTrace);

        return this;
    }
}
