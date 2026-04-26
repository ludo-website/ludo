using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IGameThemeService
{
    public Task<ServiceResponse> Add(GameThemeCreateRecord gameTheme, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GameThemeRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameThemeRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameThemeRecord>>> FilterByTheme(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
}
