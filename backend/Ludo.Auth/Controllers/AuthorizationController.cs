using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Authorization;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Auth.Controllers;

[ApiController]
[Route("auth/[controller]/[action]")]
public class AuthorizationController(ILogger<AuthorizationController> logger, ILoginService loginService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse<LoginResponseRecord>>> Login([FromBody] LoginRecord login)
    {
        return FromServiceResponse(await loginService.Login(login with { Password = PasswordUtils.HashPassword(login.Password)}));
    }
}
