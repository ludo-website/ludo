using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Clients.Abstractions;

public interface IAestheticClient
{
    public Task<ServiceResponse> Add(AestheticCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AestheticRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<AestheticRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(AestheticUpdateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
}
