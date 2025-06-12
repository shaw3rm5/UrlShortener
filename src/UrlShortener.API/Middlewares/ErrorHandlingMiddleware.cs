using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context, 
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception exception)
        {
            ProblemDetails problemDetails;

            switch (exception)
            {
                case FluentValidation.ValidationException validationException:
                    problemDetails =
                        problemDetailsFactory.CreateFrom(context, validationException);
                    break;
                case ApplicationLayerException applicationException:
                    problemDetails = problemDetailsFactory.CreateFrom(context, applicationException);
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        context,
                        StatusCodes.Status500InternalServerError,
                        "Oops! it's unhandled error, please, contact us!", exception.Message);
                    break;
            }
            
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}