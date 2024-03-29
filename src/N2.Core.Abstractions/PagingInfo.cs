namespace N2.Core;

public class PagingInfo
{
    public string Q { get; set; } = string.Empty;
    public string Sort { get; set; } = string.Empty;
    public bool SortDesc { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
