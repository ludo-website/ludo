using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class PerspectiveController(ILogger<PerspectiveController> logger, IPerspectiveService perspectiveService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] PerspectiveCreateRecord perspective)
    {
        return FromServiceResponse(await perspectiveService.Add(perspective));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<PerspectiveRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await perspectiveService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<PerspectiveRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await perspectiveService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] PerspectiveUpdateRecord perspective)
    {
        return FromServiceResponse(await perspectiveService.Update(perspective));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await perspectiveService.Delete(id));
    }
}
