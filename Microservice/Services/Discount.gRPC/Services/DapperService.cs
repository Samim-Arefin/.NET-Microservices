using Dapper;
using Discount.gRPC.Infrastructure;
using Npgsql;
using System.Data;

namespace Discount.gRPC.Services
{
    public interface IDapperService
    {
        Task<T> GetAsync<T>(string sql, object param);
        Task<T> CreateAsync<T>(string sql, object param);
        Task<int> ExecuteAsync(string sql, object param);
    }

    public class DapperService : IDapperService, IDisposable
    {
        private readonly IDbConnection _dbConnection;
        public DapperService()
            => _dbConnection = new NpgsqlConnection(AppSettings.Settings.ConnectionStrings);

        public async Task<T> GetAsync<T>(string sql, object param)
            => await _dbConnection.QueryFirstOrDefaultAsync<T>(sql: sql, param: param);

        public async Task<T> CreateAsync<T>(string sql, object param)
            => await _dbConnection.ExecuteScalarAsync<T>(sql: sql, param: param);

        public async Task<int> ExecuteAsync(string sql, object param)
            => await _dbConnection.ExecuteAsync(sql: sql, param: param);

        public void Dispose()
            => _dbConnection.Dispose();
    }
}
