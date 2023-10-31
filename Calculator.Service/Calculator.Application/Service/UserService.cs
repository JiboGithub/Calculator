using Calculator.Application.Contract;
using Calculator.Domain.Constants;
using Calculator.Domain.Entities;
using Calculator.Domain.Models.Response;
using Calculator.Infrastructure.Extensions;
using Calculator.Infrastructure.Helpers;
using Calculator.Infrastructure.Repository.Contracts;
using Dapper;
using System.Data;

namespace Calculator.Application.Service;

public class UserService : IUserService
{
    private readonly ISqlQuery _query;
    public UserService(ISqlQuery query)
    {
        _query = query;
    }

    public async Task<OperationResponse> CreateUserAsync()
    {
        User user = new()
        {
            Username = CharGen.GenerateUsername()
        };
        var parameters = new DynamicParameters();
        parameters.Add("@Username", user.Username, DbType.String);
        parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        var result = await _query.CreateOrUpdateAsync(QueryConstants.CreateUserQuery, parameters);

        user.Id = parameters.Get<int?>("@UserId") ?? 0;
        var res = result.GetSpCreateResponse<User>();
        res.ResponseData = user;
        return res;
    }

    public async Task<OperationResponse> GetUserById(int userId)
    {
        var user = await _query.GetAsync<User>(QueryConstants.GetUserByIdQuery, new { UserId = userId });
        return OperationResponse.CustomExistsResponse(user);
    }
}