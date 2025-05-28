using System.Text.Json.Serialization;

namespace N2.Core.Commands;

/// <summary>
/// The list command.
/// </summary>
public class PagedRequest<T> : CommandRequest<T> where T : class, new()
{
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Gets or sets the requested items per page.
    /// The commandhandler is not required to follow this request.
    /// In the response, the actual value for items per page, is included.
    /// </summary>
    public int ItemsPerPage { get; set; }

    /// <summary>
    /// An optional request filter.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Filter { get; set; }
}
