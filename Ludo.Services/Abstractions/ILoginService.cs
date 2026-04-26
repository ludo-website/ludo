using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface ILoginService
{
    public string GetToken(AccountRecord account, DateTime issuedAt, TimeSpan expiresIn);
}
