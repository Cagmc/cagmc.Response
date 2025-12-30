using System.Diagnostics;

namespace cagmc.Response.Core;

/// <summary>
/// Represents a response object that encapsulates the result of an operation
/// with optional data of a specific type.
/// </summary>
/// <typeparam name="T">The type of the data contained in the response.</typeparam>
[DebuggerDisplay("Success = {IsSuccess}, Code = {Code}")]
public record Response<T> : ResponseBase<Response<T>>
{
    public T? Data { get; init; }
    
    public new static Response<T> Success(T? data = default) => new() { IsSuccess = true, Code = 200, Data = data };
}
