using Ludo.Database.Repository;
using Ludo.Database.Repository.Entities;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Repositories.Interfaces;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.Constants;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Specifications;

namespace Ludo.Services.Implementations;

public class AccountService(IRepository<WebAppDatabaseContext> repository, IMailService mailService) : IAccountService
{
    public async Task<ServiceResponse> Add(AccountCreateRecord account, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(AccountSpec.GetByName(account.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Account
        {
            Name = account.Name,
            Email = account.Email,
            Password = account.Password,
            Role = account.Role
        }, cancellationToken);
        await mailService.SendMail(account.Email, "Welcome!", MailTemplates.AccountAddTemplate(account.Name), true, "My App", cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<AccountRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(AccountProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) : 
            ServiceResponse.FromError<AccountRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<int>> GetCount(CancellationToken cancellationToken = default) => 
        ServiceResponse.ForSuccess(await repository.GetCountAsync<Account>(cancellationToken));
    public async Task<ServiceResponse<AccountRecord>> Login(LoginRecord login, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(AccountSpec.GetByName(login.Name), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<AccountRecord>(CommonErrors.NotFound);
        }
        if (result.Password != login.Password)
        {
            return ServiceResponse.FromError<AccountRecord>(CommonErrors.WrongPassword);
        }
        return ServiceResponse.ForSuccess(new AccountRecord
        {
            Id = result.Id,
            Email = result.Email,
            Name = result.Name,
            Role = result.Role
        });
    }
    public async Task<ServiceResponse<PagedResponse<AccountRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, AccountProjectionSpec.FilterByName(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Update(AccountUpdateRecord account, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(AccountSpec.Get(account.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        if (account.Name != null && account.Name != entity.Name && await repository.GetAsync(AccountSpec.GetByName(account.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        entity.Name = account.Name ?? entity.Name;
        entity.Email = account.Email ?? entity.Email;
        entity.Password = account.Password ?? entity.Password;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(AccountSpec.Get(id), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Account>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
