using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Authorization;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Authorization;
using Ludo.Services.Clients.Abstractions;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(ILogger<AccountController> logger, IAccountClient accountClient) : AuthorizedController(logger, accountClient)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] AccountCreateRecord account)
    {
        account.Password = PasswordUtils.HashPassword(account.Password);
        return FromServiceResponse(await AccoutClient.Add(account));
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> AddAdmin([FromBody] AccountCreateRecord account)
    {
        var currentAccount = await GetCurrentAccount();
        account.Password = PasswordUtils.HashPassword(account.Password);
        return FromServiceResponse(await AccoutClient.Add(account, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<AccountRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await AccoutClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AccountRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await AccoutClient.FilterByName(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] AccountUpdateRecord account)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await AccoutClient.Update(account with
            {
                Password = !string.IsNullOrWhiteSpace(account.Password) ? PasswordUtils.HashPassword(account.Password) : null
            }, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await AccoutClient.Delete(id, currentAccount.Result));
    }
}
