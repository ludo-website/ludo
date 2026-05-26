using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Authorization;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class AccountController(ILogger<AccountController> logger, IAccountService accountService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] AccountCreateRecord account)
    {
        return FromServiceResponse(await accountService.Add(account));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<AccountRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await accountService.Get(id));
    }
    [HttpPost]
    public async Task<ActionResult<RequestResponse<AccountRecord>>> Login([FromBody] LoginRecord login)
    {
        return FromServiceResponse(await accountService.Login(login));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AccountRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await accountService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] AccountUpdateRecord account)
    {
        return FromServiceResponse(await accountService.Update(account with
            {
                Password = !string.IsNullOrWhiteSpace(account.Password) ? account.Password : null
            }));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await accountService.Delete(id));
    }
}
