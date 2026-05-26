using Ludo.Infrastructure.Responses;

namespace Ludo.Services.Abstractions;

public interface IMailService
{
    public Task<ServiceResponse> SendMail(string recipientEmail, string subject, string body, bool isHtmlBody = false,
        string? senderTitle = null, CancellationToken cancellationToken = default);
}
