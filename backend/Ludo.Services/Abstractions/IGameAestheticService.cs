using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IGameAestheticService
{
    public Task<ServiceResponse> Add(GameAestheticCreateRecord gameAesthetic, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GameAestheticRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByAesthetic(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
