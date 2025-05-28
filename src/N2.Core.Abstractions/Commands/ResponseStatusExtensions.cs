namespace N2.Core.Commands;

public static class ResponseStatusExtensions
{
    /// <summary>
    /// Checks if the response status indicates a successful operation.
    /// </summary>
    /// <param name="status">The response status.</param>
    /// <returns>True if the status indicates success, otherwise false.</returns>
    public static bool IsSuccess(this ResponseStatus status)
    {
        return status == ResponseStatus.Success ||
               status == ResponseStatus.Created ||
               status == ResponseStatus.Accepted ||
               status == ResponseStatus.Information ||
               status == ResponseStatus.NoContent ||
               status == ResponseStatus.ResetContent;
    }

    public static string ToMessage(this ResponseStatus status)
    {
        return status switch
        {
            ResponseStatus.Success => "The operation was successful.",
            ResponseStatus.Created => "The resource was created successfully.",
            ResponseStatus.Accepted => "The request has been accepted for processing.",
            ResponseStatus.Information => "The request was accepted and is being processed.",
            ResponseStatus.NoContent => "The request was successful but there is no content to return.",
            ResponseStatus.ResetContent => "The content has been reset successfully.",
            ResponseStatus.NotAccepted => "The request was not accepted.",
            ResponseStatus.NotModified => "The resource has not been modified since the last request.",
            ResponseStatus.BadRequest => "The request was invalid or cannot be served.",
            ResponseStatus.Unauthorized => "Authentication is required and has failed or has not yet been provided.",
            ResponseStatus.Forbidden => "The server understood the request, but refuses to authorize it.",
            ResponseStatus.NotFound => "The requested resource could not be found.",
            ResponseStatus.NotAcceptable => "The requested resource is not available in a format acceptable to the client.",
            ResponseStatus.TimeOut => "The server timed out waiting for the request.",
            ResponseStatus.Conflict => "There was a conflict with the current state of the resource.",
            ResponseStatus.Locked => "The resource is locked and cannot be modified at this time.",
            ResponseStatus.ToomanyRequests => "Too many requests have been made in a given amount of time.",
            ResponseStatus.ServerError => "An internal server error occurred while processing the request.",
            ResponseStatus.NotImplemented => "The server does not support the functionality required to fulfill the request.",
            ResponseStatus.ServiceUnavailable => "The service is currently unavailable, please try again later.",
            _ => "Unknown response status."
        };
    }
}