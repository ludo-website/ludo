using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;
using Ludo.Infrastructure.Configurations;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Clients.Abstractions;

namespace Ludo.Services.Implementations;

public class LoginService(IAccountClient accoutClient, IOptions<JwtConfiguration> jwtConfiguration) : ILoginService
{
    private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration.Value;
    private readonly IAccountClient AccoutClient = accoutClient;
    public async Task<ServiceResponse<LoginResponseRecord>> Login(LoginRecord login, CancellationToken cancellationToken = default)
    {
        var account = await AccoutClient.Login(login, cancellationToken);
        if (account.Result == null)
        {
            return ServiceResponse.FromError<LoginResponseRecord>(account.Error);
        }
        return ServiceResponse.ForSuccess(new LoginResponseRecord
        {
            Account = account.Result,
            Token = GetToken(account.Result, DateTime.UtcNow, new(7, 0, 0, 0))
        });
    }
    private string GetToken(AccountRecord account, DateTime issuedAt, TimeSpan expiresIn)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new([new(ClaimTypes.NameIdentifier, account.Id.ToString())]),
            Claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, account.Name },
                { ClaimTypes.Email, account.Email }
            },
            IssuedAt = issuedAt,
            Expires = issuedAt.Add(expiresIn),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}
