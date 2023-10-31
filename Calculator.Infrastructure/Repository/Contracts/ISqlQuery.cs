using System.Data;

namespace Calculator.Infrastructure.Repository.Contracts;

public interface ISqlQuery
{
    Task<int> CreateOrUpdateAsync<T>(string query, T? obj, IDbTransaction? dbTransaction = null, int? timeout = null, CommandType? commandType = CommandType.StoredProcedure);
    Task<T?> GetAsync<T>(string query, object? obj = null, CommandType? commandType = null);
    Task<IEnumerable<T?>> GetAllAsync<T>(string query, object? obj = null, CommandType? commandType = CommandType.StoredProcedure);
    IEnumerable<T1> GetAllAsync<T1, T2>(string query, Func<T1, T2, T1> map, object? obj, CommandType? commandType, string splitter);
}
