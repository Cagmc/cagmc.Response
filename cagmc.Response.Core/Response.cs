namespace cagmc.Response.Core;

/// <summary>
/// Represents a standard response object that encapsulates the result of an operation,
/// including status information and an optional message.
/// </summary>
public record Response
{
    public bool IsSuccess { get; init; }
    public int Code { get; init; }
    public string? Message { get; init; }
    
    public bool IsSuccessStatusCode => Code is >= 200 and < 300;
    public bool IsClientError => Code is >= 400 and < 500;
    public bool IsServerError => Code is >= 500 and < 600;
    
    public static Response Success => new() { IsSuccess = true, Code = 200 };
    public static Response BadRequest => new() { IsSuccess = false, Code = 400 };
    public static Response Unauthorized => new() { IsSuccess = false, Code = 401 };
    public static Response Forbidden => new() { IsSuccess = false, Code = 403 };
    public static Response NotFound => new() { IsSuccess = false, Code = 404 };
    public static Response Conflict => new() { IsSuccess = false, Code = 409 };
    public static Response InternalServerError => new() { IsSuccess = false, Code = 500 };
    
    public static Response CreateError(int code, string? message = null) => new() { IsSuccess = false, Code = code, Message = message };
    public static Response CreateBadRequest(string? message = null) => new() { IsSuccess = false, Code = 400, Message = message };
    
    public static Response FromException(Exception ex) => new() { IsSuccess = false, Code = 500, Message = ex.Message };
}
