using Calculator.Domain.Models.Response;

namespace Calculator.Application.Contract
{
    public interface IUserService
    {
        Task<OperationResponse> CreateUserAsync();
        Task<OperationResponse> GetUserById(int userId);
    }
}