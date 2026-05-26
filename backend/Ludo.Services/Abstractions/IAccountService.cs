using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IAccountService
{
    public Task<ServiceResponse> Add(AccountCreateRecord account, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AccountRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<int>> GetCount(CancellationToken cancellationToken = default);
    public Task<ServiceResponse<AccountRecord>> Login(LoginRecord login, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<AccountRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(AccountUpdateRecord account, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
