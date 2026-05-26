using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class ModeController(ILogger<ModeController> logger, IModeService modeService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ModeCreateRecord mode)
    {
        return FromServiceResponse(await modeService.Add(mode));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ModeRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await modeService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ModeRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await modeService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ModeUpdateRecord mode)
    {
        return FromServiceResponse(await modeService.Update(mode));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await modeService.Delete(id));
    }
}
