using Calculator.Domain.Enums;

namespace Calculator.Application.Dtos.Request
{
    public class CalculationRequest
    {
        public int? UserId { get; set; }
        public double FirstValue { get; set; }
        public double SecondValue { get; set; }
        public required OperationTypeEnum OperationType { get; set; }
    }
}
