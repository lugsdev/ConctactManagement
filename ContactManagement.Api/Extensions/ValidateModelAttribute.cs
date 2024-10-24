using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ContactManagement.Api.Extensions.Models;
using System.Net;

namespace ContactManagement.Api.Extensions
{
    public class ValidateModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors
                        .Select(e => $"Parâmetro inválido para o campo '{x.Key}': {e.ErrorMessage}"))
                    .ToList();

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 400,
                    Errors = errors
                };

                context.Result = new BadRequestObjectResult(errorResponse);

            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
