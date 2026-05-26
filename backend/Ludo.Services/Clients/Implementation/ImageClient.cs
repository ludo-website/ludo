using System.Net.Http.Json;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.DataTransferObjects;
using Newtonsoft.Json;

namespace Ludo.Services.Clients.Implementation;

public class ImageClient(HttpClient httpClient, IPostClient postClient, IGameClient gameClient) : IImageClient
{
    public async Task<ServiceResponse> Add(ImageCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamedev)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamedevAdd);
        }
        var post = await postClient.Get(entity.PostId, cancellationToken);
        if (post.Result == null)
        {
            return ServiceResponse.FromError(post.Error);
        }
        var game = await gameClient.Get(post.Result.GameId, cancellationToken);
        if (game.Result == null)
        {
            return ServiceResponse.FromError(game.Error);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && game.Result.AccountId != requestingAccount.Id)
        {
            return ServiceResponse.FromError(CommonErrors.NonOwnerAdd);
        }
        var result = await httpClient.PostAsJsonAsync("Add", entity, cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(CommonErrors.TechnicalSupport);
        }
        var textString = await result.Content.ReadAsStringAsync(cancellationToken);
        var response = JsonConvert.DeserializeObject<ClientResponse>(textString);
        if (response == null)
        {
            return ServiceResponse.FromError(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(response);
    }
    private async Task<ServiceResponse<ImageRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var textString = await httpClient.GetStringAsync($"Get/{id}", cancellationToken);
        var result = JsonConvert.DeserializeObject<ClientResponse<ImageRecord>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<ImageRecord>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<ImageRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<ImageRecord>(CommonErrors.Unauthenticated);
        }
        return await Get(id, cancellationToken);
    }
    public async Task<ServiceResponse<FileRecord>> Download(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<FileRecord>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync($"Download/{id}", cancellationToken);
        var result = JsonConvert.DeserializeObject<ClientResponse<FileRecord>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<FileRecord>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<PagedResponse<ImageRecord>>> FilterByPost(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<ImageRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByPost?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<ImageRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<ImageRecord>>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamedev)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamedevDelete);
        }
        var image = await Get(id, cancellationToken);
        if (image.Result == null)
        {
            return ServiceResponse.FromError(image.Error);
        }
        var post = await postClient.Get(image.Result.PostId, cancellationToken);
        if (post.Result == null)
        {
            return ServiceResponse.FromError(post.Error);
        }
        var game = await gameClient.Get(post.Result.GameId, cancellationToken);
        if (game.Result == null)
        {
            return ServiceResponse.FromError(game.Error);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && game.Result.AccountId != requestingAccount.Id)
        {
            return ServiceResponse.FromError(CommonErrors.NonOwnerDelete);
        }
        var result = await httpClient.DeleteAsync($"Delete/{id}", cancellationToken);
        var textString = await result.Content.ReadAsStringAsync(cancellationToken);
        var response = JsonConvert.DeserializeObject<ClientResponse>(textString);
        if (response == null)
        {
            return ServiceResponse.FromError(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(response);
    }
}
