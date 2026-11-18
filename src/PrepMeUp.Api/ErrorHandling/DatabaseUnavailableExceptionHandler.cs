using System.Data.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PrepMeUp.Api.ErrorHandling;

public class DatabaseUnavailableExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<DatabaseUnavailableExceptionHandler> logger) : IExceptionHandler
{
    public const string Message =
        "PrepMeUp cannot reach its database right now. Please try again in a few minutes.";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (!IsDatabaseError(exception))
        {
            return false;
        }

        logger.LogError(exception, "The database could not be reached");

        httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status503ServiceUnavailable,
                Title = "The database is unavailable.",
                Detail = Message,
            },
        });
    }

    private static bool IsDatabaseError(Exception? exception)
    {
        for (; exception is not null; exception = exception.InnerException)
        {
            if (exception is DbException)
            {
                return true;
            }
        }

        return false;
    }
}
