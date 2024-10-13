using Shared.API.Response;
using System.Linq.Expressions;

namespace Catalog.API.Services.Interfaces
{
    public interface ICommonService<T> where T : class
    {
        Task<List<U>?> GetAllAsync<U>(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default);
        Task<U?> GetByIdAsync<U>(string id, CancellationToken cancellationToken = default);
        Task<U?> FindOneAsync<U>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task<U?> InsertOneAsync<U>(U uEntity, T? tEntity = null, CancellationToken cancellationToken = default);
        Task<Unit> ReplaceOneAsync<U>(string id, U uEntity, T? tEntity = null, CancellationToken cancellationToken = default);
        Task<Unit> DeleteOneAsync(string id, CancellationToken cancellationToken = default);
        Task<Unit> DeleteManyAsync(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default);
    }
}
