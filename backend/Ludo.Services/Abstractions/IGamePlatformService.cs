using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IGamePlatformService
{
    public Task<ServiceResponse> Add(GamePlatformCreateRecord gamePlatform, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GamePlatformRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GamePlatformRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GamePlatformRecord>>> FilterByPlatform(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
