using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IGamePerspectiveService
{
    public Task<ServiceResponse> Add(GamePerspectiveCreateRecord gamePerspective, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GamePerspectiveRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GamePerspectiveRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GamePerspectiveRecord>>> FilterByPerspective(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
