using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IImageService
{
    public const string ImagesDirectory = "Images";
    public Task<ServiceResponse> Add(ImageCreateRecord image, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<FileRecord>> Download(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<ImageRecord>> Get(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<PagedResponse<ImageRecord>>> FilterByPost(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default);
}
