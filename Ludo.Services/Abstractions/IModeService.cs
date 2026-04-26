using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IModeService
{
    public Task<ServiceResponse> Add(ModeCreateRecord mode, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<ModeRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<ModeRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(ModeUpdateRecord mode, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
}
