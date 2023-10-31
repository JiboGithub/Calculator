using Microsoft.AspNetCore.Mvc;

namespace Calculator.Infrastructure.Validation
{
    public interface IValidationExceptionHandler
    {
        IActionResult HandleValidationErrors(List<string> errors);
    }
}
