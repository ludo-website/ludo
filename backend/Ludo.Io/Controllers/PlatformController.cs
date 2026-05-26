using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class PlatformController(ILogger<PlatformController> logger, IPlatformService platformService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] PlatformCreateRecord platform)
    {
        return FromServiceResponse(await platformService.Add(platform));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<PlatformRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await platformService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<PlatformRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await platformService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] PlatformUpdateRecord platform)
    {
        return FromServiceResponse(await platformService.Update(platform));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await platformService.Delete(id));
    }
}
