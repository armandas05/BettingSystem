using BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BettingSystem.Filters;

public sealed class BettingExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BettingValidationException exception)
        {
            context.Result = new BadRequestObjectResult(exception.Message);
            context.ExceptionHandled = true;
        }
    }
}
