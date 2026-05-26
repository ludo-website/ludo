using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Clients.Abstractions;

public interface IAccountClient
{
    public Task<ServiceResponse> Add(AccountCreateRecord entity, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AccountRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AccountRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AccountRecord>> Login(LoginRecord login, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<AccountRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(AccountUpdateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default);
}
