namespace cagmc.Response.Core;

/// <summary>
/// Represents a response containing a list of data and associated metadata such as total count.
/// </summary>
/// <typeparam name="T">The type of the elements in the list.</typeparam>
public record ListResponse<T> : Response<List<T>>
{
    public ListResponse() : this([])
    {
    }
    
    public ListResponse(List<T> data) : this(data, data.Count)
    {
    }

    public ListResponse(List<T> data, int total)
    {
        Data = data;
        Total = total;
        IsSuccess = true;
        Code = 200;
    }
    
    public int Total { get; init; }
}