namespace N2.Core.Commands;

/// <summary>
/// A paged response.
/// </summary>
public interface IPagedResponse<T> : ICommandResponse
{
    /// <summary>
    /// Gets the items.
    /// </summary>
    IList<T> Items { get; }

    /// <summary>
    /// Gets or sets the items per page.
    /// </summary>
    int ItemsPerPage { get; }

    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Gets or sets the total item count.
    /// </summary>
    int TotalItemCount { get; }

    /// <summary>
    /// Gets or sets the total pages.
    /// </summary>
    int TotalPages { get; }
}
