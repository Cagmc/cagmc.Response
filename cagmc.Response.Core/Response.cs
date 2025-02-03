namespace cagmc.Response.Core;

public record Response
{
    public bool IsSuccess { get; init; }
    public int Code { get; init; }
    public string? Message { get; init; }
    
    public static Response Success => new() { IsSuccess = true, Code = 200 };
    public static Response BadRequest => new() { IsSuccess = false, Code = 400 };
    public static Response NotFound => new() { IsSuccess = false, Code = 404 };
    
    public static Response CreateError(int code, string? message = null) => new() { IsSuccess = false, Code = code, Message = message };
    public static Response CreateBadRequest(string? message = null) => new() { IsSuccess = false, Code = 400, Message = message };
}
