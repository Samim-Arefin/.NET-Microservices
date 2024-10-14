using Shared.API.Response;

namespace Cart.API.Services.Interfaces
{
    public interface IRedisCacheService
    {
        Task<Unit> SetAsync<T>(string key, T entity, CancellationToken cancellationToken = default);
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task<Unit> RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
