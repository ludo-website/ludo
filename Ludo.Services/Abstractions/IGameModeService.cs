using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IGameModeService
{
    public Task<ServiceResponse> Add(GameModeCreateRecord gameMode, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GameModeRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameModeRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameModeRecord>>> FilterByMode(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
}
