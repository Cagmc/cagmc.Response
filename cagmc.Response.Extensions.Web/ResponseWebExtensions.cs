using System.Text.Json;
using cagmc.Response.Core;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc;

public static class ResponseWebExtensions
{
    /// <summary>
    /// Converts a <see cref="Response"/> object into an <see cref="IActionResult"/> instance.
    /// </summary>
    /// <param name="response">The <see cref="Response"/> object to be converted into an <see cref="IActionResult"/>.</param>
    /// <returns>An <see cref="IActionResult"/> containing the serialized <see cref="Response"/> object
    /// and an HTTP status code corresponding to the <c>Code</c> property of the <see cref="Response"/>.</returns>
    public static IActionResult ToActionResult(this Response response)
    {
        return new ObjectResult(response) { StatusCode = response.Code };
    }

    /// <summary>
    /// Converts a <see cref="Response"/> object into an <see cref="IResult"/> instance suitable for Minimal API usage.
    /// </summary>
    /// <param name="response">The <see cref="Response"/> object to be converted into an <see cref="IResult"/>.</param>
    /// <returns>An <see cref="IResult"/> representing the serialized <see cref="Response"/> object
    /// with an HTTP status code reflecting the <c>Code</c> property of the <see cref="Response"/>.</returns>
    public static IResult ToMinimalApiResult(this Response response)
    {
        return new StatusCodeWithResponseResult(response.Code, response);
    }
}

/// <summary>
/// Represents a custom implementation of <see cref="IResult"/> that encapsulates an HTTP status code
/// and a response payload, intended for usage in Minimal APIs.
/// </summary>
/// <remarks>
/// This class sets the HTTP response status code and serializes the provided data object into JSON format.
/// It is useful for scenarios where you need to return both a specific HTTP status code and a response body
/// in Minimal API action results.
/// </remarks>
/// <param name="statusCode">The HTTP status code to be included in the response.</param>
/// <param name="data">The data object to be serialized as the response body.</param>
public class StatusCodeWithResponseResult(int statusCode, object data) : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        
        var json = JsonSerializer.Serialize(data);
        
        await httpContext.Response.WriteAsync(json);
    }
}