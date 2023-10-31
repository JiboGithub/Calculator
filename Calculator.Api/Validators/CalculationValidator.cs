using Calculator.Application.Dtos.Request;
using Calculator.Domain.Enums;
using FluentValidation;

namespace Calculator.Api.Validators
{
    public class CalculationValidator : AbstractValidator<CalculationRequest>
    {
        public CalculationValidator()
        {
            RuleFor(x => x.UserId)
            .NotNull().WithMessage("UserId is required.");

            RuleFor(x => x.FirstValue)
            .NotNull().WithMessage("FirstValue is required.");

            RuleFor(x => x.SecondValue)
                .NotNull().WithMessage("SecondValue is required.")
                .NotEqual(0).When(x => x.OperationType == OperationTypeEnum.DIVIDE)
                .WithMessage("SecondValue cannot be zero when performing a division.");

            RuleFor(x => x.OperationType)
                .NotNull().WithMessage("OperationType is required.")
                .IsInEnum().WithMessage("Invalid OperationType.");
        }
    }
}
