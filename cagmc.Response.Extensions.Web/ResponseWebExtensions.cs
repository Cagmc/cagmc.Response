using cagmc.Response.Core;

namespace Microsoft.AspNetCore.Mvc;

public static class ResponseWebExtensions
{
    public static IActionResult ToActionResult(this Response response)
    {
        return new ObjectResult(response) { StatusCode = response.Code };
    }
}