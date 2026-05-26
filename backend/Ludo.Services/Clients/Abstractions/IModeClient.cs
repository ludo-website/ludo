using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Clients.Abstractions;

public interface IModeClient
{
    public Task<ServiceResponse> Add(ModeCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<ModeRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<ModeRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(ModeUpdateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
}
