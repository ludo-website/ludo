using System.Net.Http.Json;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.DataTransferObjects;
using Newtonsoft.Json;

namespace Ludo.Services.Clients.Implementation;

public class InterestClient(HttpClient httpClient) : IInterestClient
{
    public async Task<ServiceResponse> Add(InterestCreateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Gamer && requestingAccount.Role != AccountRoleEnum.Admin)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamerAdd);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && entity.AccountId != requestingAccount.Id)
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
    private async Task<ServiceResponse<InterestRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var textString = await httpClient.GetStringAsync($"Get/{id}", cancellationToken);
        var result = JsonConvert.DeserializeObject<ClientResponse<InterestRecord>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<InterestRecord>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<InterestRecord>> Get(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<InterestRecord>(CommonErrors.Unauthenticated);
        }
        return await Get(id, cancellationToken);
    }
    public async Task<ServiceResponse<PagedResponse<InterestRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<InterestRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByGame?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<InterestRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<InterestRecord>>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse<PagedResponse<InterestRecord>>> FilterByAccount(PaginationIdSearchQueryParams pagination, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError<PagedResponse<InterestRecord>>(CommonErrors.Unauthenticated);
        }
        var textString = await httpClient.GetStringAsync(
            $"FilterByAccount?Search={pagination.Search}&Page={pagination.Page}&PageSize={pagination.PageSize}",
            cancellationToken
        );
        var result = JsonConvert.DeserializeObject<ClientResponse<PagedResponse<InterestRecord>>>(textString);
        if (result == null)
        {
            return ServiceResponse.FromError<PagedResponse<InterestRecord>>(CommonErrors.TechnicalSupport);
        }
        return ServiceResponse.FromClientResponse(result);
    }
    public async Task<ServiceResponse> Update(InterestUpdateRecord entity, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamer)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamerUpdate);
        }
        var interest = await Get(entity.Id, cancellationToken);
        if (interest.Result == null)
        {
            return ServiceResponse.FromError(interest.Error);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && interest.Result.AccountId != requestingAccount.Id)
        {
            return ServiceResponse.FromError(CommonErrors.NonOwnerUpdate);
        }
        var result = await httpClient.PutAsJsonAsync("Update", entity, cancellationToken);
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
    public async Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamer)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamerDelete);
        }
        var interest = await Get(id, cancellationToken);
        if (interest.Result == null)
        {
            return ServiceResponse.FromError(interest.Error);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Id != interest.Result.AccountId)
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
