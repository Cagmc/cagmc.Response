using System.Diagnostics;

namespace cagmc.Response.Core;

/// <summary>
/// Represents a response object derived from a base response structure, designed to encapsulate
/// the status and outcome of an operation.
/// </summary>
[DebuggerDisplay("Success = {IsSuccess}, Code = {Code}")]
public record Response : ResponseBase<Response>
{
}
