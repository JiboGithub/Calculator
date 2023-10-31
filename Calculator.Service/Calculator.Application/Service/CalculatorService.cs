using Calculator.Application.Contract;
using Calculator.Application.Dtos.Request;
using Calculator.Application.Dtos.Response;
using Calculator.Domain.Constants;
using Calculator.Domain.Entities;
using Calculator.Domain.Enums;
using Calculator.Domain.Models.Response;
using Calculator.Infrastructure.Repository.Contracts;
using Mapster;

namespace Calculator.Application.Service;

public class CalculatorService : ICalculatorService
{
    private readonly ISqlQuery _query;

    public CalculatorService(ISqlQuery query)
    {
        _query = query;
    }

    public async Task SaveCalculationAsync(CalculationHistory calculationHistory)
    {
        _ = await _query.CreateOrUpdateAsync(QueryConstants.SaveCalculationQuery, calculationHistory);
    }

    public async Task<OperationResponse> GetCalculationHistoryByUserIdAsync(int userId)
    {
        var historyItems = await _query.GetAllAsync<CalculationHistoryResponse>(QueryConstants.GetCalculationHistoryQuery, new { UserId = userId });
        return OperationResponse.CustomExistsResponse(historyItems);
    }

    public async Task<OperationResponse> PerformCalculationAsync(CalculationRequest request)
    {
        double result;

        switch (request.OperationType)
        {
            case OperationTypeEnum.ADD:
                result = request.FirstValue + request.SecondValue;
                break;
            case OperationTypeEnum.SUBTRACT:
                result = request.FirstValue - request.SecondValue;
                break;
            case OperationTypeEnum.MULTIPLY:
                result = request.FirstValue * request.SecondValue;
                break;
            case OperationTypeEnum.DIVIDE:
                if (request.SecondValue == 0)
                    throw new DivideByZeroException("Operand2 cannot be zero when performing division.");
                result = request.FirstValue / request.SecondValue;
                break;
            default:
                throw new NotSupportedException($"The operation '{request.OperationType}' is not supported.");
        }

        var calculationHistory = request.Adapt<CalculationHistory>();
        calculationHistory.Result = result;
        await SaveCalculationAsync(calculationHistory);

        var data = new CalculationResponse
        {
            Result = result
        };
        return OperationResponse.CreatedResponse(data);
    }
}
