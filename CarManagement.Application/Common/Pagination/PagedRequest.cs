namespace CarManagement.Application.Common.Pagination;

public sealed class PagedRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
}
