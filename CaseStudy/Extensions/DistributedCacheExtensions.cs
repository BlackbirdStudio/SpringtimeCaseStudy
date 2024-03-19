using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace CaseStudy.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<TData>(this IDistributedCache cache, string recordId, TData data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpiredTime = null)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = unusedExpiredTime
            };

            var jsonData = JsonSerializer.Serialize(data);
            var jsonDataBytes = Encoding.UTF8.GetBytes(jsonData);
            await cache.SetAsync(recordId, jsonDataBytes, options);
        }

        public static async Task<TData> GetRecordAsync<TData>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if(jsonData is null)
            {
                return default!;
            }
            return JsonSerializer.Deserialize<TData>(jsonData)!;
        }
    }
}
