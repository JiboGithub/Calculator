using Calculator.Application.Dtos.Request;
using Calculator.Domain.Entities;
using Calculator.Domain.Models.Response;

namespace Calculator.Application.Contract
{
    public interface ICalculatorService
    {
        Task<OperationResponse> PerformCalculationAsync(CalculationRequest request);
        Task<OperationResponse> GetCalculationHistoryByUserIdAsync(int userId);
        Task SaveCalculationAsync(CalculationHistory calculationHistory);
    }
}