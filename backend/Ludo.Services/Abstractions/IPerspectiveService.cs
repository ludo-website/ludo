using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IPerspectiveService
{
    public Task<ServiceResponse> Add(PerspectiveCreateRecord perspective, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PerspectiveRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<PerspectiveRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(PerspectiveUpdateRecord perspective, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
