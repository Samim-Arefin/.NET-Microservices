using Cart.API.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Shared.API.Response;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Cart.API.Services.Implementations
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _redisCache;
        public RedisCacheService(IDistributedCache  redisCache) => 
            _redisCache = redisCache;
       
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var hasValue = await Haskey(key, cancellationToken);
            if (string.IsNullOrEmpty(hasValue))
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(hasValue, SerializerOptions());
        }

        public async Task<Unit> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            var hasValue = await Haskey(key, cancellationToken);
            if (string.IsNullOrEmpty(hasValue))
            {
                throw new BadHttpRequestException($"{key} not found!");
            }

            await _redisCache.RemoveAsync(key, cancellationToken);

            return new(statusCode: HttpStatusCodes.OK, isSuccess: true, message: "Successfully removed!");
        }

        public async Task<Unit> SetAsync<T>(string key, T entity, CancellationToken cancellationToken = default)
        {
            var hasValue = await Haskey(key, cancellationToken);
            var value = JsonSerializer.Serialize<T>(entity, SerializerOptions());
            await _redisCache.SetStringAsync(key, value, DstributedCacheEntryOptions());

            return string.IsNullOrEmpty(hasValue)
            ?
            new(statusCode: HttpStatusCodes.OK, isSuccess: true, message: "Successfully added!")
            : 
            new(statusCode: HttpStatusCodes.OK, isSuccess: true, message: "Successfully updated!");
        }

        private async Task<string?> Haskey(string key, CancellationToken cancellationToken) => 
            await _redisCache.GetStringAsync(key, cancellationToken);

        private DistributedCacheEntryOptions DstributedCacheEntryOptions() => 
            new()
            { 
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3),
            };

        private JsonSerializerOptions SerializerOptions() => 
            new()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
    }
}
