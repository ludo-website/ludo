using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Clients.Abstractions;

public interface IGameAestheticClient
{
    public Task<ServiceResponse> Add(GameAestheticCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GameAestheticRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByAesthetic(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
}
