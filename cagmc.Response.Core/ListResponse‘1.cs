namespace cagmc.Response.Core;

/// <summary>
/// Represents a response that contains a list of items along with the total count of items.
/// </summary>
/// <typeparam name="T">The type of items contained in the response.</typeparam>
public record ListResponse<T>
{
    /// <summary>
    /// Represents a response that provides a list of items with metadata such as total count, page index, and page size.
    /// </summary>
    /// <typeparam name="T">The type of items included in the response.</typeparam>
    public ListResponse() : this([])
    {
    }

    /// <summary>
    /// Represents a generic response containing a list of items, along with additional information such as total count, page index, and page size.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the list.</typeparam>
    public ListResponse(List<T> items) : this(items, items.Count)
    {
    }

    /// <summary>
    /// Represents a response that contains a list of items, along with metadata such as the total count, page index, and page size.
    /// </summary>
    /// <typeparam name="T">The type of the items included in the response.</typeparam>
    public ListResponse(List<T> items, int total, int? pageIndex = null, int? pageSize = null)
    {
        Items = items;
        Total = total;
        PageIndex = pageIndex ?? 1;
        PageSize = pageSize ?? items.Count;
    }

    /// <summary>
    /// Gets the list of items included in the response. This property contains the collection of items
    /// returned as part of the response. The type of items is defined by the generic parameter T.
    /// </summary>
    public List<T> Items { get; init; }

    /// <summary>
    /// Gets the total count of items available in the response. This property represents the total number of items
    /// regardless of the current pagination settings, providing the overall item count in the list.
    /// </summary>
    public int Total { get; init; }

    /// <summary>
    /// Gets the index of the current page in the paginated response.
    /// This property represents the one-based index of the page being returned.
    /// </summary>
    public int PageIndex { get; init; }

    /// <summary>
    /// Gets the number of items included in each page of the response. This property specifies the maximum number of items that can be returned in a single page.
    /// </summary>
    public int PageSize { get; init; }
}