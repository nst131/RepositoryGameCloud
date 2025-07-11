using Microsoft.AspNetCore.Mvc.Filters;
using TranslaterWebApi.Exceptions;

namespace TranslaterWebApi.Filters
{
    public class ValidateModelFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var error = context.ModelState
                    .FirstOrDefault()
                    .Value?.Errors?.FirstOrDefault()?.ErrorMessage;

                throw new ValidateModelException($"{error}");
            }

            await next();
        }
    }
}
