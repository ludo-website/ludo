using System.Net.Http.Json;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.DataTransferObjects;
using Newtonsoft.Json;

namespace Ludo.Services.Clients.Implementation;

public class GameAestheticClient(HttpClient httpClient, IGameClient gameClient) : IGameAestheticClient
{
    public async Task<ServiceResponse> Add(GameAestheticCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamedev)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamedevAdd);
        }
        var game = await gameClient.Get(entity.GameId, cancellationToken);
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
    private async Task<ServiceResponse<GameAestheticRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var textString = await httpClient.GetStringAsync($"Get/{id}", cancellationToken);
        var result = JsonConvert.DeserializeObject<ClientResponse<GameAestheticRecord>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<GameAestheticRecord>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<GameAestheticRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<GameAestheticRecord>(CommonErrors.Unauthenticated);
        }
        return await Get(id, cancellationToken);
    }
    public async Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameAestheticRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByGame?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<GameAestheticRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameAestheticRecord>>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByAesthetic(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameAestheticRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByAesthetic?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<GameAestheticRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameAestheticRecord>>(CommonErrors.TechnicalSupport);
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
        var gameAesthetic = await Get(id, cancellationToken);
        if (gameAesthetic.Result == null)
        {
            return ServiceResponse.FromError(gameAesthetic.Error);
        }
        var game = await gameClient.Get(gameAesthetic.Result.GameId, cancellationToken);
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
