using Microsoft.AspNetCore.Mvc;

namespace Calculator.Infrastructure.Validation
{
    public class ValidationExceptionHandler : IValidationExceptionHandler
    {
        public IActionResult HandleValidationErrors(List<string> errors)
        {
            return new BadRequestObjectResult(errors);
        }
    }


}
