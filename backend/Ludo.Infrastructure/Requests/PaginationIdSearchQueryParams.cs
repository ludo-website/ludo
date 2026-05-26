namespace Ludo.Infrastructure.Requests;

public class PaginationIdSearchQueryParams : PaginationQueryParams
{
    public Guid? Search { get; set; }
}
