using System.Net;
using Ludo.Database.Repository;
using Ludo.Database.Repository.Entities;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Repositories.Interfaces;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Specifications;

namespace Ludo.Services.Implementations;

public class ImageService(IRepository<WebAppDatabaseContext> repository, IFileRepository fileRepository) : IImageService
{
    private static string GetFileDirectory(Guid userId) => Path.Join(userId.ToString(), IImageService.ImagesDirectory);
    public async Task<ServiceResponse<PagedResponse<ImageRecord>>> GetImages(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ImageProjectionSpec(pagination.Search), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> SaveImage(ImageCreateRecord file, AccountRecord requestingUser, CancellationToken cancellationToken = default)
    {
        var fileName = fileRepository.SaveFile(file.File, GetFileDirectory(requestingUser.Id));

        if (fileName.Result == null)
        {
            return fileName.ToResponse();
        }

        await repository.AddAsync(new Image
        {
            Name = file.File.FileName,
            Path = fileName.Result,
            PostId = requestingUser.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<FileRecord>> GetImageDownload(Guid id, CancellationToken cancellationToken = default) // If not successful respond with the error.
    {
        var image = await repository.GetAsync<Image>(id, cancellationToken); // First get the file entity from the database to find the location on the filesystem.

        return image != null ? 
            fileRepository.GetFile(Path.Join(GetFileDirectory(image.PostId), image.Path), image.Name) : 
            ServiceResponse.FromError<FileRecord>(new(HttpStatusCode.NotFound, "File entry not found!", ErrorCodes.NotFound));
    }
}
