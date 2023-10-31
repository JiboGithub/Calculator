using Calculator.Domain.Enums;

namespace Calculator.Domain.Models.Response
{
    public class CalculationHistoryResponse
    {
        public int UserId { get; set; }
        public double FirstValue { get; set; }
        public double SecondValue { get; set; }
        public string? OperationType { get; set; }
        public double Result { get; set; }
    }
}
