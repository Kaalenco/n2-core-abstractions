namespace N2.Core.Commands;

/// <summary>
/// The paged response.
/// </summary>
public class PagedResponse<T> : CommandResponse, IPagedResponse<T>
{
    /// <summary>
    ///
    /// </summary>
    public const int DefaultPageSize = 25;

    /// <summary>
    /// Gets or sets the sequential number (starting with 1) of the current page.
    /// A value of zero, indicates that no paging is provided.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int ItemsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of available pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total item count.
    /// </summary>
    public int TotalItemCount { get; set; }

    /// <summary>
    /// Gets or sets the items collection.
    /// </summary>
    public IList<T> Items { get; private set; } = new List<T>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResponse{T}"/> class.
    /// </summary>
    /// <param name="count">The total number of items in the set</param>
    /// <param name="page">The current page</param>
    /// <param name="ipp">The number of items per page</param>
    /// <param name="items">The items</param>
    public PagedResponse(int count, int page, int ipp, IEnumerable<T> items)
    {
        if (count <= 0)
        {
            count = 0;
        }

        if (page <= 0)
        {
            page = 1;
        }

        if (ipp <= 0)
        {
            ipp = DefaultPageSize;
        }

        Page = page;
        TotalPages = (count + ipp) / ipp;
        ItemsPerPage = ipp;
        TotalItemCount = count;
        foreach (T? item in items)
        {
            Items.Add(item);
        }
        Status = ResponseStatus.Success;
    }

    /// <summary>
    /// Create a new Paged Response
    /// </summary>
    public PagedResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResponse{T}"/> class.
    /// </summary>
    /// <param name="items">Initialize the items list.</param>
    public PagedResponse(IEnumerable<T> items)
    {
        foreach (T? item in items)
        {
            Items.Add(item);
        }
        Page = 1;
        TotalPages = 1;
        ItemsPerPage = Items.Count;
        TotalItemCount = ItemsPerPage;
        Status = ResponseStatus.Success;
    }
}
