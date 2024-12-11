namespace Pos.App.BaseModule.Interfaces.Shared;

public interface ICacheService
{
    Task<bool> ExistRedisCache(string cacheKey);
    Task<T> GetRedisCache<T>(string cacheKey);
    Task WriteRedisCache(string cacheKey, string cacheValue);
    Task UpdateRedisCache(string cacheKey, string cacheValue);
    Task DeleteCache(string cacheKey);
}