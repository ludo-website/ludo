using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class InterestController(ILogger<InterestController> logger, IInterestService interestService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] InterestCreateRecord interest)
    {
        return FromServiceResponse(await interestService.Add(interest));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<InterestRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await interestService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<InterestRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await interestService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<InterestRecord>>>> FilterByAccount([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await interestService.FilterByAccount(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] InterestUpdateRecord interest)
    {
        return FromServiceResponse(await interestService.Update(interest));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await interestService.Delete(id));
    }
}
