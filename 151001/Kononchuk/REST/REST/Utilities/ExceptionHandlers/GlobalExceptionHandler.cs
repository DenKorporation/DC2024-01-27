using Microsoft.AspNetCore.Diagnostics;
using REST.Models.DTOs.Response;
using REST.Utilities.Exceptions;

namespace REST.Utilities.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        int code;
        ErrorResponseDto? errorMessage;
        switch (exception)
        {
            case ResourceNotFoundException notFoundException:
                code = 404;
                errorMessage = new ErrorResponseDto()
                {
                    ErrorMessage = notFoundException.Message, ErrorCode = notFoundException.Code
                };
                break;
            case ValidationException validationException:
                code = 400;
                errorMessage = new ErrorResponseDto()
                {
                    ErrorMessage = validationException.Message, ErrorCode = validationException.Code
                };
                break;
            case UniqueConstraintException uniqueConstraintException:
                code = 409;
                errorMessage = new ErrorResponseDto()
                {
                    ErrorMessage = uniqueConstraintException.Message, ErrorCode = uniqueConstraintException.Code
                };
                break;
            default:
                return false;
        }

        httpContext.Response.StatusCode = code;

        await httpContext.Response
            .WriteAsJsonAsync(errorMessage, cancellationToken);

        return true;
    }
}