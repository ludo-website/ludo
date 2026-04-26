using System.Text.Json.Serialization;

namespace Ludo.Infrastructure.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCodes
{
    NotFound,
    WrongPassword,
    Unauthenticated,
    Duplicate,
    CannotAdd,
    CannotUpdate,
    CannotDelete,
    Unknown,
    TechnicalError,
    MailSendFailed
}
