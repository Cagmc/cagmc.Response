namespace cagmc.Response.Core;

public record Response<T> : Response
{
    public T? Data { get; init; }
}