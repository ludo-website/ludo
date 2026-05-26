using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Responses;

namespace Ludo.Infrastructure.Handlers;

public abstract class BaseResponseController(ILogger logger) : ControllerBase
{   
    protected ActionResult<RequestResponse> ErrorMessageResult(ServerException serverException) =>
        StatusCode((int)serverException.Status, RequestResponse.FromError(ErrorMessage.FromException(serverException).LogError(logger)));

    protected ActionResult<RequestResponse> ErrorMessageResult(ErrorMessage? errorMessage = null) =>
        StatusCode((int)(errorMessage?.Status ?? HttpStatusCode.InternalServerError), RequestResponse.FromError(errorMessage?.LogError(logger)));

    protected ActionResult<RequestResponse<T>> ErrorMessageResult<T>(ErrorMessage? errorMessage = null) =>
        StatusCode((int)(errorMessage?.Status ?? HttpStatusCode.InternalServerError), RequestResponse<T>.FromError(errorMessage?.LogError(logger)));

    protected ActionResult<RequestResponse> FromServiceResponse(ServiceResponse response) =>
        response.Error == null ? Ok(RequestResponse.OkRequestResponse) : ErrorMessageResult(response.Error);
    
    protected async Task<ActionResult<RequestResponse>> FromServiceResponse(Task<ServiceResponse> response) =>
        FromServiceResponse(await response);

    protected ActionResult<RequestResponse<T>> FromServiceResponse<T>(ServiceResponse<T> response) =>
        response.Error == null ? Ok(RequestResponse<T>.FromServiceResponse(response)) : ErrorMessageResult<T>(response.Error);
    
    protected ActionResult<RequestResponse<FileRecord>> FromServiceResponse(ServiceResponse<FileRecord> response) =>
        response.Result != null ? File(response.Result.Stream, MediaTypeNames.Application.Octet, fileDownloadName: response.Result.Name) : // The File method of the controller base returns a response from a stream with the given media type and filename.
            ErrorMessageResult<FileRecord>(response.Error);
    
    protected ActionResult<RequestResponse<FileRecord>> FromServiceResponse(ServiceResponse<FileRecord> response, string contentType) =>
        response.Result != null ? File(response.Result.Stream, contentType, fileDownloadName: response.Result.Name) : 
            ErrorMessageResult<FileRecord>(response.Error);
    
    protected async Task<ActionResult<RequestResponse<T>>> FromServiceResponse<T>(Task<ServiceResponse<T>> response) =>
        FromServiceResponse(await response);

    protected ActionResult<RequestResponse> OkRequestResponse() => Ok(RequestResponse.OkRequestResponse);
}