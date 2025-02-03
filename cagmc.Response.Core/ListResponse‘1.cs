namespace cagmc.Response.Core;

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