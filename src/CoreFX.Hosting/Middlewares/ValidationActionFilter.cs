using CoreFX.Hosting.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreFX.Hosting.Middlewares
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Result = modelState.ToErrorJson400();
                base.OnActionExecuting(context);
            }
        }
    }
}
