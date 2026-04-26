using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Authorization;

public abstract class AuthorizedController(ILogger logger, IAccountService accountService) : BaseResponseController(logger)
{
    private AccountClaims? _accountClaims;
    protected readonly IAccountService AccountService = accountService;

    protected AccountClaims ExtractClaims()
    {
        if (_accountClaims != null)
        {
            return _accountClaims;
        }
        var enumerable = User.Claims.ToList();
        var id = enumerable.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => Guid.Parse(x.Value)).FirstOrDefault();
        var email = enumerable.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
        var name = enumerable.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
        _accountClaims = new(id, name, email);
        return _accountClaims;
    }

    protected Task<ServiceResponse<AccountRecord>> GetCurrentAccount() => AccountService.Get(ExtractClaims().Id);
}
