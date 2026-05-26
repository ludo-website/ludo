using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IInterestService
{
    public Task<ServiceResponse> Add(InterestCreateRecord interest, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<InterestRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<InterestRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<InterestRecord>>> FilterByAccount(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(InterestUpdateRecord interest, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
