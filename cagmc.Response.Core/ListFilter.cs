namespace cagmc.Response.Core;

public record ListFilter
{
    public string? Search { get; init; }
    
    public string? SortByColumn { get; init; }
    public bool? IsAscending { get; init; } = true;
    
    public int? PageIndex { get; init; }
    public int? PageSize { get; init; }
}