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
    private static string GetFileDirectory(Guid postId) => Path.Join(postId.ToString(), IImageService.ImagesDirectory);
    public async Task<ServiceResponse> Add(ImageCreateRecord file, CancellationToken cancellationToken = default)
    {
        var post = await repository.GetAsync(PostSpec.Get(file.PostId), cancellationToken);
        if (post == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var game = await repository.GetAsync(GameSpec.Get(post.GameId), cancellationToken);
        if (game == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var fileName = fileRepository.SaveFile(file.File, GetFileDirectory(file.PostId));
        if (fileName.Result == null)
        {
            return fileName.ToResponse();
        }
        await repository.AddAsync(new Image
        {
            Name = file.File.FileName,
            Path = fileName.Result,
            PostId = file.PostId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<ImageRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(ImageProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ImageRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<FileRecord>> Download(Guid id, CancellationToken cancellationToken = default)
    {
        var image = await repository.GetAsync<Image>(id, cancellationToken);
        return image != null ? 
            fileRepository.GetFile(Path.Join(GetFileDirectory(image.PostId), image.Path), image.Name) :
            ServiceResponse.FromError<FileRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<ImageRecord>>> FilterByPost(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, ImageProjectionSpec.FilterByPost(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var image = await repository.GetAsync<Image>(id, cancellationToken);
        if (image == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var post = await repository.GetAsync(PostSpec.Get(image.PostId), cancellationToken);
        if (post == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var game = await repository.GetAsync(GameSpec.Get(post.GameId), cancellationToken);
        if (game == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        fileRepository.DeleteFile(Path.Join(GetFileDirectory(image.PostId), image.Path));
        await repository.DeleteAsync<Image>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
