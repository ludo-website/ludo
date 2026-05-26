using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Abstractions;

public interface ILoginService
{
    public Task<ServiceResponse<LoginResponseRecord>> Login(LoginRecord login, CancellationToken cancellationToken = default);
}
