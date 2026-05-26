using System.Net.Http.Json;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.DataTransferObjects;
using Newtonsoft.Json;

namespace Ludo.Services.Clients.Implementation;

public class GameGenreClient(HttpClient httpClient, IGameClient gameClient) : IGameGenreClient
{
    public async Task<ServiceResponse> Add(GameGenreCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
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
    private async Task<ServiceResponse<GameGenreRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var textString = await httpClient.GetStringAsync($"Get/{id}", cancellationToken);
        var result = JsonConvert.DeserializeObject<ClientResponse<GameGenreRecord>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<GameGenreRecord>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<GameGenreRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<GameGenreRecord>(CommonErrors.Unauthenticated);
        }
        return await Get(id, cancellationToken);
    }
    public async Task<ServiceResponse<PagedResponse<GameGenreRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameGenreRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByGame?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<GameGenreRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameGenreRecord>>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<PagedResponse<GameGenreRecord>>> FilterByGenre(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameGenreRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByGenre?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<GameGenreRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<GameGenreRecord>>(CommonErrors.TechnicalSupport);
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
        var gameGenre = await Get(id, cancellationToken);
        if (gameGenre.Result == null)
        {
            return ServiceResponse.FromError(gameGenre.Error);
        }
        var game = await gameClient.Get(gameGenre.Result.GameId, cancellationToken);
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
