using cagmc.Response.Core;

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
}