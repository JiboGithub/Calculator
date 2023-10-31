using Calculator.Domain.Models.Configuration;
using Calculator.Infrastructure.Repository.Contracts;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace Calculator.Infrastructure.Repository.Implementation;

public class SqlQuery : ISqlQuery
{
    private readonly IDbConnection _db;
    private DbConfig _config;

    public IDbConnection CreateConnection(IOptions<DbConfig> config)
    {
        _config = config.Value;
        var connectionString = _config.SqlConnection;
        return new SqlConnection(connectionString);
    }
    public SqlQuery(IOptions<DbConfig> config)
    {
        _db ??= CreateConnection(config);
    }

    public Task<T?> GetAsync<T>(string query, object? obj = null, CommandType? commandType = null)
    {
        return _db.QueryFirstOrDefaultAsync<T?>(query, obj, commandType: commandType);
    }

    public Task<IEnumerable<T?>> GetAllAsync<T>(string query, object? obj, CommandType? commandType)
    {
        return _db.QueryAsync<T?>(query, obj, commandType: commandType);
    }

    public IEnumerable<T1> GetAllAsync<T1, T2>(string query, Func<T1, T2, T1> map, object? obj, CommandType? commandType, string splitter)
    {
        return _db.Query(query, map,
        commandType: commandType,
        param: obj,
        splitOn: splitter);
    }

    public Task<int> CreateOrUpdateAsync<T>(string query, T? obj, IDbTransaction? dbTransaction, int? timeout, CommandType? commandType = CommandType.StoredProcedure)
    {
        return _db.ExecuteAsync(query, obj, dbTransaction, timeout, commandType);
    }
}