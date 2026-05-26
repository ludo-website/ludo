namespace Ludo.Infrastructure.Responses;

public class PagedResponse<T>(int page, int pageSize, int totalCount, List<T> data)
{
    public int Page { get; set; } = page;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;
    public List<T> Data { get; set; } = data;
}

public static class PagedResponseExtension
{
    public static PagedResponse<TOut> Map<TIn, TOut>(this PagedResponse<TIn> response, Func<TIn, TOut> selector) =>
        new(response.Page, response.PageSize, response.TotalCount, response.Data.Select(selector).ToList());
}