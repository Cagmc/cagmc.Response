using System.Net;

namespace cagmc.Response.Core;

public abstract record ResponseBase<TCode, TResponse>
    where TResponse : ResponseBase<TCode, TResponse>
{
    public bool IsSuccess { get; init; }
    public TCode Code { get; init; } = default!;
    public string? Message { get; init; }
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
    public Dictionary<string, string[]>? ValidationErrors { get; init; }
    public string? TraceId { get; init; }

    public TResponse WithMessage(string message) => (TResponse)this with { Message = message };
    public TResponse WithTraceId(string traceId) => (TResponse)this with { TraceId = traceId };
    public TResponse WithValidationErrors(Dictionary<string, string[]> validationErrors) => (TResponse)this with { ValidationErrors = validationErrors };

    public void EnsureSuccess()
    {
        if (!IsSuccess) throw new ResponseUnsucceededException(
            $"The operation failed with code '{Code}' and message '{Message}'.");
    }
}

/// <summary>
/// Serves as a generic base class for defining structured responses,
/// providing common properties and methods to standardize the handling of operation outcomes.
/// </summary>
/// <typeparam name="TResponse">The specific type of the response derived from this base class.</typeparam>
public abstract record ResponseBase<TResponse> : ResponseBase<int, ResponseBase<TResponse>>
    where TResponse : ResponseBase<TResponse>, new()
{
    public bool IsSuccessStatusCode => Code is >= 200 and < 300;
    public bool IsClientError => Code is >= 400 and < 500;
    public bool IsServerError => Code is >= 500 and < 600;
    
    public static TResponse Success => new() { IsSuccess = true, Code = 200 };
    public static TResponse BadRequest => new() { Code = 400 };
    public static TResponse Unauthorized => new() { Code = 401 };
    public static TResponse Forbidden => new() { Code = 403 };
    public static TResponse NotFound => new() { Code = 404 };
    public static TResponse Conflict => new() { Code = 409 };
    public static TResponse InternalServerError => new() { Code = 500 };
    
    public static TResponse CreateError(int code, string? message = null) => new() { Code = code, Message = message };
    public static TResponse CreateBadRequest(string? message = null) => new() { Code = 400, Message = message };
    
    public static TResponse FromException(Exception ex) => new() { Code = 500, Message = ex.Message };
}

public abstract record ResponseBaseHttp<TResponse> : ResponseBase<HttpStatusCode, ResponseBaseHttp<TResponse>>
    where TResponse : ResponseBaseHttp<TResponse>, new()
{
    public bool IsSuccessStatusCode => (int)Code is >= 200 and < 300;
    public bool IsClientError => (int)Code is >= 400 and < 500;
    public bool IsServerError => (int)Code is >= 500 and < 600;
    
    public static TResponse Success => new() { IsSuccess = true, Code = HttpStatusCode.OK };
    public static TResponse BadRequest => new() { Code = HttpStatusCode.BadRequest };
    public static TResponse Unauthorized => new() { Code = HttpStatusCode.Unauthorized };
    public static TResponse Forbidden => new() { Code = HttpStatusCode.Forbidden };
    public static TResponse NotFound => new() { Code = HttpStatusCode.NotFound };
    public static TResponse Conflict => new() { Code = HttpStatusCode.Conflict };
    public static TResponse InternalServerError => new() { Code = HttpStatusCode.InternalServerError };
    
    public static TResponse CreateError(HttpStatusCode code, string? message = null) => new() { Code = code, Message = message };
    public static TResponse CreateBadRequest(string? message = null) => new() { Code = HttpStatusCode.BadRequest, Message = message };
    
    public static TResponse FromException(Exception ex) => new() { Code = HttpStatusCode.InternalServerError, Message = ex.Message };
}

public abstract record ResponseBaseString<TResponse> : ResponseBase<string, ResponseBaseString<TResponse>>
    where TResponse : ResponseBaseString<TResponse>, new()
{
    public bool IsSuccessStatusCode => Code == "success";
    public bool IsClientError => Code == "bad_request";
    public bool IsServerError => Code == "internal_server_error";
    
    public static TResponse Success => new() { IsSuccess = true, Code = "success" };
    public static TResponse BadRequest => new() { Code = "bad_request" };
    public static TResponse InternalServerError => new() { Code = "internal_server_error" };
    
    public static TResponse CreateError(string code, string? message = null) => new() { Code = code, Message = message };
    public static TResponse CreateBadRequest(string? message = null) => new() { Code = "bad_request", Message = message };
    
    public static TResponse FromException(Exception ex) => new() { Code = "internal_server_error", Message = ex.Message };
}
