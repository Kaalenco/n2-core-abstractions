using N2.Core.Commands;

namespace N2.Core.Entity;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Complete the unit of work, which typically means committing the changes to the database.
    /// </summary>
    /// <returns>Status and an optional message. Usually, the message is only provided when the completion resulted in an error.</returns>
    Task<(ResponseStatus status, string? message)> Complete();

    /// <summary>
    /// Gets a value indicating whether the unit of work is configured correctly and can accept changes.
    /// This is typically true when a transaction is initialized and ready to process commands.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Checks the health of the data context, which includes verifying the connection to the database
    /// </summary>
    DataContextHealthStatus Health();
}
