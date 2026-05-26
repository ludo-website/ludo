using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IPlatformService
{
    public Task<ServiceResponse> Add(PlatformCreateRecord platform, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PlatformRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<PlatformRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Update(PlatformUpdateRecord platform, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
