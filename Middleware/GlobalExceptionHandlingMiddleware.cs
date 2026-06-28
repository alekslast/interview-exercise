using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewExercise.Middleware;

public class GlobalExceptionHandlingMiddleware(
    ILogger<GlobalExceptionHandlingMiddleware> logger
) : IMiddleware
{
    readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


            ProblemDetails problem = new();


            switch (ex)
            {

                default:
                    _logger.LogError(ex, "An unhandled exception has occurred: {ErrorMessage}", ex.Message);

                    problem.Status = (int)HttpStatusCode.InternalServerError;
                    problem.Type = "Server Error";
                    problem.Detail = "An internal server error has occured. Please contact System Administrator.";

                    break;
            }


            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}

