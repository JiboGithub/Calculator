using Calculator.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;

namespace Calculator.Application.Filter
{
    public class ValidationFilter : IActionFilter
    {
        private readonly IValidationServiceFactory _validationServiceFactory;
        private readonly IValidationExceptionHandler _validationExceptionHandler;
        public ValidationFilter(
            IValidationServiceFactory validationServiceFactory,
            IValidationExceptionHandler validationExceptionHandler)
        {
            _validationServiceFactory = validationServiceFactory;
            _validationExceptionHandler = validationExceptionHandler;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            var contextArgs = context.ActionArguments;

            if (contextArgs is null || contextArgs.Count < 1)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }
            var param = contextArgs
                .SingleOrDefault(x =>
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    return x.Value.ToString().ToLower()
                                    .Contains("request");
                }).Value;

            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var paramType = param.GetType();

#pragma warning restore CS8602 // Dereference of a possibly null reference.
            object? validationServiceType = typeof(IValidationServiceFactory)?
                .GetMethod("GetValidationService")?
                .MakeGenericMethod(paramType)
                .Invoke(_validationServiceFactory, null);

            dynamic? validationService = validationServiceType;

            // Find the Validate method of the ValidationService dynamically
            var validateMethod = validationService?.GetType().GetMethod("Validate");

            // Invoke the Validate method dynamically with the param object
            IEnumerable? validationErrors = (IEnumerable)validateMethod!.Invoke(validationService, new object[] { param });

            // Cast the validationErrors to List<string>
            var errorList = validationErrors.Cast<string>().ToList();

            if (errorList.Any())
            {
                context.Result = _validationExceptionHandler.HandleValidationErrors(errorList);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
