using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class ThemeController(ILogger<ThemeController> logger, IThemeService themeService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ThemeCreateRecord theme)
    {
        return FromServiceResponse(await themeService.Add(theme));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ThemeRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await themeService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ThemeRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await themeService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ThemeUpdateRecord theme)
    {
        return FromServiceResponse(await themeService.Update(theme));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await themeService.Delete(id));
    }
}
