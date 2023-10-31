using Calculator.Domain.Enums;

namespace Calculator.Domain.Models.Response
{
    public class CalculationHistoryRequest
    {
        public int UserId { get; set; }
        public double FirstValue { get; set; }
        public double SecondValue { get; set; }
        public OperationTypeEnum OperationType { get; set; }
    }
}
