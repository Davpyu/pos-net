using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Pos.App.BaseModule.Interfaces.Shared;
using Pos.Configs;

namespace Pos.BaseModule.Services;

public class CacheService : ICacheService
{
    private IDistributedCache _distributedCache;
    private readonly AppSettings _appSettings;
    private string _serviceName;

    public CacheService(
        IDistributedCache distributedCache,
        IOptions<AppSettings> appSettings
        )
    {
        _distributedCache = distributedCache;
        _appSettings = appSettings.Value;
        _serviceName = $"{_appSettings.ServiceName}_";
    }

    public async Task<bool> ExistRedisCache(string cacheKey)
    {
        byte[] cacheValue = null;
        try
        {
            cacheValue = await _distributedCache.GetAsync(_serviceName + cacheKey);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Read Error : {0}", e.Message);
        }

        return cacheValue != null;
    }

    public async Task<string> GetRedisCache(string cacheKey)
    {
        byte[] cacheValue = null;

        try
        {
            cacheValue = await _distributedCache.GetAsync(_serviceName + cacheKey);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Read Error : {0}", e.Message);
        }

        if (cacheValue != null)
        {
            return Encoding.UTF8.GetString(cacheValue);
        }

        return default;
    }


    public async Task<T> GetRedisCache<T>(string cacheKey)
    {
        byte[] cacheValue = null;

        try
        {
            cacheValue = await _distributedCache.GetAsync(_serviceName + cacheKey);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Read Error : {0}", e.Message);
        }

        if (cacheValue != null)
        {
            string cacheValueStr = Encoding.UTF8.GetString(cacheValue);
            return JsonSerializer.Deserialize<T>(cacheValueStr);
        }

        return default;
    }

    public async Task WriteRedisCache(string cacheKey, string cacheValue)
    {
        try
        {
            var cacheData = Encoding.UTF8.GetBytes(cacheValue);

            // Set cache options.
            var options = new DistributedCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetAbsoluteExpiration(DateTime.Now.AddDays(7)).SetSlidingExpiration(TimeSpan.FromHours(6));

            // Save data in cache.
            await _distributedCache.SetAsync(_serviceName + cacheKey, cacheData, options);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Write Error : {0}", e.Message);
        }
    }

    public async Task WriteRedisCachePermanent(string cacheKey, string cacheValue)
    {
        try
        {
            byte[] cacheData = Encoding.UTF8.GetBytes(cacheValue);

            // Save data in cache.
            await _distributedCache.SetAsync(_serviceName + cacheKey, cacheData);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Write Error : {0}", e.Message);
        }
    }

    public async Task UpdateRedisCache(string cacheKey, string cacheValue)
    {
        try
        {
            await DeleteCache(cacheKey);
            await WriteRedisCache(cacheKey, cacheValue);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Update Error : {0}", e.Message);
        }
    }

    public async Task DeleteCache(string cacheKey)
    {
        try
        {
            await _distributedCache.RemoveAsync(_serviceName + cacheKey);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis Read Error : {0}", e.Message);
        }
    }
}