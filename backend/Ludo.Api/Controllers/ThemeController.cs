using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.Authorization;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ThemeController(ILogger<ThemeController> logger, IAccountClient accountClient, IThemeClient themeClient) : AuthorizedController(logger, accountClient)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ThemeCreateRecord theme)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await themeClient.Add(theme, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ThemeRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await themeClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ThemeRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await themeClient.FilterByName(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ThemeUpdateRecord theme)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await themeClient.Update(theme, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await themeClient.Delete(id, currentAccount.Result));
    }
}
