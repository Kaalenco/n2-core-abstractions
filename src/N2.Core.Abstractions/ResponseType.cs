namespace N2.Core;

public enum ResponseType
{
    None = 0,
    Success = 200,
    Created = 201,
    NoChange = 203,
    NoContent = 204,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    NotAccepted = 406,
    Conflict = 409,
    InternalServerError = 500
}