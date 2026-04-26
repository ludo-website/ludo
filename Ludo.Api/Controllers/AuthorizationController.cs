using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Authorization;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthorizationController(ILogger<AuthorizationController> logger, IAccountService accountService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse<LoginResponseRecord>>> Login([FromBody] LoginRecord login)
    {
        return FromServiceResponse(await accountService.Login(login with { Password = PasswordUtils.HashPassword(login.Password)}));
    }
}
