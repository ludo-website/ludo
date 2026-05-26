using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Clients.Abstractions;

public interface IGenreClient
{
    public Task<ServiceResponse> Add(GenreCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<GenreRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<GenreRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(GenreUpdateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
}
