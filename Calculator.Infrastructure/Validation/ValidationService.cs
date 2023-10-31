using FluentValidation;

namespace Calculator.Infrastructure.Validation
{
    public class ValidationService<T> : IValidationService<T>
    {
        private readonly IValidator<T> _validator;

        public ValidationService(IValidator<T> validator)
        {
            _validator = validator;
        }

        public List<string> Validate(T model)
        {
            var validationResult = _validator.Validate(model);
            return validationResult.Errors.Select(error => error.ErrorMessage).ToList();
        }
    }

}
