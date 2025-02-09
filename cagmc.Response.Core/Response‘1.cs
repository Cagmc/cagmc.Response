namespace cagmc.Response.Core;

/// <summary>
/// Represents a response object that encapsulates the result of an operation
/// with optional data of a specific type.
/// </summary>
/// <typeparam name="T">The type of the data contained in the response.</typeparam>
public record Response<T> : ResponseBase<Response<T>>
{
    public T? Data { get; init; }
}
