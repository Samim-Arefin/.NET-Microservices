using Dapper;
using Microsoft.Data.SqlClient;
using Ordering.Application.Common.Service;
using Ordering.Infrastructure.Extensions;
using System.Data;

namespace Ordering.Infrastructure.Services
{
    public class DapperService : IDapperService, IDisposable
    {
        private readonly SqlConnection _sqlConnection;
        public DapperService()
            => _sqlConnection = new SqlConnection(AppSettings.Settings.ConnectionStrings);

        public async Task<List<T>> ExecuteQuery<T>(string procedure)
        {
           var result = await _sqlConnection.QueryAsync<T>(procedure, commandType: CommandType.StoredProcedure);
           return result.ToList();
        }

        public async Task<List<T>> ExecuteQueryWithParam<T, U>(string procedure, U parameter)
        {
            var result = await _sqlConnection.QueryAsync<T>(procedure, parameter, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<T> ExecuteSingleQuery<T, U>(string procedure, U parameter) 
            => await _sqlConnection.QueryFirstOrDefaultAsync<T>(procedure, parameter, commandType: CommandType.StoredProcedure);

        public async Task<T> ExecuteNonQuery<T, U>(string procedure, U parameter)
        {
            var result = await _sqlConnection.ExecuteScalarAsync<T>(procedure, parameter, commandType: CommandType.StoredProcedure);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task ExecuteNonQuery<U>(string procedure, U parameter) 
            => await _sqlConnection.ExecuteAsync(procedure, parameter, commandType: CommandType.StoredProcedure);

        public void Dispose()
          => _sqlConnection.Dispose();
    }
}
