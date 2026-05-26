using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class AestheticController(ILogger<AestheticController> logger, IAestheticService aestheticService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] AestheticCreateRecord aesthetic)
    {
        return FromServiceResponse(await aestheticService.Add(aesthetic));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<AestheticRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await aestheticService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<AestheticRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await aestheticService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] AestheticUpdateRecord aesthetic)
    {
        return FromServiceResponse(await aestheticService.Update(aesthetic));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await aestheticService.Delete(id));
    }
}
