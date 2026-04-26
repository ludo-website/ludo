using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface IImageService
{
    public const string ImagesDirectory = "Images";
    public Task<ServiceResponse<PagedResponse<ImageRecord>>> GetImages(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> SaveImage(ImageCreateRecord image, AccountRecord requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<FileRecord>> GetImageDownload(Guid id, CancellationToken cancellationToken = default);
}
