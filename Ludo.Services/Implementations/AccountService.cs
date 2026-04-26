using Ludo.Database.Repository;
using Ludo.Database.Repository.Entities;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Repositories.Interfaces;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.Constants;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Specifications;

namespace Ludo.Services.Implementations;

public class AccountService(IRepository<WebAppDatabaseContext> repository, ILoginService loginService, IMailService mailService)
    : IAccountService
{
    public async Task<ServiceResponse<LoginResponseRecord>> Login(LoginRecord login, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AccountSpec(login.Email), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<LoginResponseRecord>(CommonErrors.NotFound);
        }
        if (result.Password != login.Password)
        {
            return ServiceResponse.FromError<LoginResponseRecord>(CommonErrors.WrongPassword);
        }
        var account = new AccountRecord
        {
            Id = result.Id,
            Email = result.Email,
            Name = result.Name,
            Role = result.Role
        };
        return ServiceResponse.ForSuccess(new LoginResponseRecord
        {
            Account = account,
            Token = loginService.GetToken(account, DateTime.UtcNow, new(7, 0, 0, 0))
        });
    }
    public async Task<ServiceResponse> Add(AccountCreateRecord account, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (account.Role == AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Admin)
        {
            return ServiceResponse.FromError(CommonErrors.NonAdminAdd);
        }
        if (await repository.GetAsync(new AccountSpec(account.Email), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        if (await repository.GetAsync(new AccountSpec(account.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Account
        {
            Email = account.Email,
            Name = account.Name,
            Role = account.Role,
            Password = account.Password
        }, cancellationToken);
        await mailService.SendMail(account.Email, "Welcome!", MailTemplates.UserAddTemplate(account.Name), true, "My App", cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<AccountRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new AccountProjectionSpec(id), cancellationToken);
        return result != null ? 
            ServiceResponse.ForSuccess(result) : 
            ServiceResponse.FromError<AccountRecord>(CommonErrors.NotFound);
    }

    public async Task<ServiceResponse<PagedResponse<AccountRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new AccountProjectionSpec(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> Update(AccountUpdateRecord account, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Id != account.Id)
        {
            return ServiceResponse.FromError(CommonErrors.NonOwnerUpdate);
        }
        var entity = await repository.GetAsync(new AccountSpec(account.Id), cancellationToken);
        if (entity != null)
        {
            entity.Name = account.Name ?? entity.Name;
            entity.Password = account.Password ?? entity.Password;

            await repository.UpdateAsync(entity, cancellationToken);
        }
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingUser = null, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingUser.Role != AccountRoleEnum.Admin && requestingUser.Id != id)
        {
            return ServiceResponse.FromError(CommonErrors.NonOwnerDelete);
        }
        await repository.DeleteAsync<Account>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
