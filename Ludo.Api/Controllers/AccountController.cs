using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Authorization;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.Authorization;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(ILogger<AccountController> logger, IAccountService accountService) : AuthorizedController(logger, accountService)
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<AccountRecord>>> GetById([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();

        return currentAccount.Result != null ? 
            FromServiceResponse(await AccountService.Get(id)) : 
            ErrorMessageResult<AccountRecord>(currentAccount.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AccountRecord>>>> GetPage([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();

        return currentAccount.Result != null ?
            FromServiceResponse(await AccountService.FilterByName(pagination)) :
            ErrorMessageResult<PagedResponse<AccountRecord>>(currentAccount.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] AccountCreateRecord account)
    {
        var currentAccount = await GetCurrentAccount();
        account.Password = PasswordUtils.HashPassword(account.Password);

        return currentAccount.Result != null ?
            FromServiceResponse(await AccountService.Add(account, currentAccount.Result)) :
            ErrorMessageResult(currentAccount.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] AccountUpdateRecord account)
    {
        var currentAccount = await GetCurrentAccount();

        return currentAccount.Result != null ?
            FromServiceResponse(await AccountService.Update(account with
            {
                Password = !string.IsNullOrWhiteSpace(account.Password) ? PasswordUtils.HashPassword(account.Password) : null
            }, currentAccount.Result)) :
            ErrorMessageResult(currentAccount.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();

        return currentAccount.Result != null ?
            FromServiceResponse(await AccountService.Delete(id)) :
            ErrorMessageResult(currentAccount.Error);
    }
}
