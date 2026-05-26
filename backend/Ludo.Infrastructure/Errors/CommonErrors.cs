using System.Net;

namespace Ludo.Infrastructure.Errors;

public static class CommonErrors
{
    public static ErrorMessage NotFound => new(HttpStatusCode.NotFound, "Entity doesn't exist!", ErrorCodes.NotFound);
    public static ErrorMessage WrongPassword => new(HttpStatusCode.BadRequest, "Wrong password!", ErrorCodes.WrongPassword);
    public static ErrorMessage Unauthenticated => new(HttpStatusCode.Unauthorized, "Not logged in!", ErrorCodes.Unauthenticated);
    public static ErrorMessage NonAdminAdd => new(HttpStatusCode.Forbidden, "Only an admin can add!", ErrorCodes.CannotAdd);
    public static ErrorMessage NonOwnerAdd => new(HttpStatusCode.Forbidden, "Only the owner can add!", ErrorCodes.CannotAdd);
    public static ErrorMessage NonGamedevAdd => new(HttpStatusCode.Forbidden, "Only the gamedev can add!", ErrorCodes.CannotAdd);
    public static ErrorMessage NonGamerAdd => new(HttpStatusCode.Forbidden, "Only the gamer can add!", ErrorCodes.CannotAdd);
    public static ErrorMessage NonAdminUpdate => new(HttpStatusCode.Forbidden, "Only an admin can update!", ErrorCodes.CannotUpdate);
    public static ErrorMessage NonOwnerUpdate => new(HttpStatusCode.Forbidden, "Only the owner can update!", ErrorCodes.CannotUpdate);
    public static ErrorMessage NonGamedevUpdate => new(HttpStatusCode.Forbidden, "Only the gamedev can update!", ErrorCodes.CannotUpdate);
    public static ErrorMessage NonGamerUpdate => new(HttpStatusCode.Forbidden, "Only the gamer can update!", ErrorCodes.CannotUpdate);
    public static ErrorMessage NonAdminDelete => new(HttpStatusCode.Forbidden, "Only an admin can delete!", ErrorCodes.CannotDelete);
    public static ErrorMessage NonOwnerDelete => new(HttpStatusCode.Forbidden, "Only the owner can delete!", ErrorCodes.CannotDelete);
    public static ErrorMessage NonGamedevDelete => new(HttpStatusCode.Forbidden, "Only the gamedev can delete!", ErrorCodes.CannotDelete);
    public static ErrorMessage NonGamerDelete => new(HttpStatusCode.Forbidden, "Only the gamer can delete!", ErrorCodes.CannotDelete);
    public static ErrorMessage Duplicate => new(HttpStatusCode.Conflict, "Entity already exists!", ErrorCodes.Duplicate);
    public static ErrorMessage AssociateNotFound => new(HttpStatusCode.NotFound, "Associated entity doesn't exist!", ErrorCodes.NotFound);
    public static ErrorMessage AssociateRoleConflict => new(HttpStatusCode.Conflict, "Associated entity has conflicting role!", ErrorCodes.RoleConflict);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);
}
