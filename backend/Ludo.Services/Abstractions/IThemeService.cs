using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IThemeService
{
    public Task<ServiceResponse> Add(ThemeCreateRecord theme, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<ThemeRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<ThemeRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(ThemeUpdateRecord theme, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
