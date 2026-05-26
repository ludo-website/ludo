using System.Text.Json.Serialization;

namespace Ludo.Infrastructure.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCodes
{
    WrongPassword,
    NotFound,
    Unauthenticated,
    RoleConflict,
    Duplicate,
    CannotAdd,
    CannotUpdate,
    CannotDelete,
    Unknown,
    TechnicalError,
    MailSendFailed
}
