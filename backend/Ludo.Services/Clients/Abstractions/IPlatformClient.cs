using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Clients.Abstractions;

public interface IPlatformClient
{
    public Task<ServiceResponse> Add(PlatformCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PlatformRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<PlatformRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(PlatformUpdateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
}
