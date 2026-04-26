using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IGameGenreService
{
    public Task<ServiceResponse> Add(GameGenreCreateRecord gameGenre, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GameGenreRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameGenreRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GameGenreRecord>>> FilterByGenre(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
}
