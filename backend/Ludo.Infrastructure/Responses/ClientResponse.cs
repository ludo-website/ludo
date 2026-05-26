using Ludo.Infrastructure.Errors;

namespace Ludo.Infrastructure.Responses;

public record ClientResponse
{
    public ErrorMessage? ErrorMessage { get; set; }
}

public sealed record ClientResponse<T>
{
    public ErrorMessage? ErrorMessage { get; internal init; }
    public T? Response { get; init; }
}
