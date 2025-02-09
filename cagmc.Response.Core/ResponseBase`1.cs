namespace cagmc.Response.Core;

/// <summary>
/// Serves as a generic base class for defining structured responses,
/// providing common properties and methods to standardize the handling of operation outcomes.
/// </summary>
/// <typeparam name="TResponse">The specific type of the response derived from this base class.</typeparam>
public abstract record ResponseBase<TResponse> where TResponse : ResponseBase<TResponse>, new()
{
    public bool IsSuccess { get; init; }
    public int Code { get; init; }
    public string? Message { get; init; }
    
    public bool IsSuccessStatusCode => Code is >= 200 and < 300;
    public bool IsClientError => Code is >= 400 and < 500;
    public bool IsServerError => Code is >= 500 and < 600;
    
    public static TResponse Success => new() { IsSuccess = true, Code = 200 };
    public static TResponse BadRequest => new() { IsSuccess = false, Code = 400 };
    public static TResponse Unauthorized => new() { IsSuccess = false, Code = 401 };
    public static TResponse Forbidden => new() { IsSuccess = false, Code = 403 };
    public static TResponse NotFound => new() { IsSuccess = false, Code = 404 };
    public static TResponse Conflict => new() { IsSuccess = false, Code = 409 };
    public static TResponse InternalServerError => new() { IsSuccess = false, Code = 500 };
    
    public static TResponse CreateError(int code, string? message = null) => new() { IsSuccess = false, Code = code, Message = message };
    public static TResponse CreateBadRequest(string? message = null) => new() { IsSuccess = false, Code = 400, Message = message };
    
    public static TResponse FromException(Exception ex) => new() { IsSuccess = false, Code = 500, Message = ex.Message };
} 
