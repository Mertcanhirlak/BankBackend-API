using BankBackend.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankBackend.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase? _cacheDb;
        private readonly bool _isAvailable;

        public CacheService(IConfiguration configuration)
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection") ?? "localhost:6379");
                _cacheDb = redis.GetDatabase();
                _isAvailable = true;
            }
            catch
            {
                // Redis bağlantısı başarısız olursa uygulama çökmesin, cache devre dışı kalsın.
                _isAvailable = false;
                _cacheDb = null;
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (!_isAvailable || _cacheDb == null) return default;

            try
            {
                var value = await _cacheDb.StringGetAsync(key);
                if (value.HasValue)
                {
                    return JsonSerializer.Deserialize<T>(value!);
                }
            }
            catch { /* Cache okuma hatası yutulabilir */ }
            
            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            if (!_isAvailable || _cacheDb == null) return;

            try
            {
                var options = new JsonSerializerOptions { ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles };
                var serializedValue = JsonSerializer.Serialize(value, options);

                if (expirationTime.HasValue)
                {
                    await _cacheDb.StringSetAsync(key, serializedValue, expirationTime.Value);
                }
                else
                {
                    await _cacheDb.StringSetAsync(key, serializedValue);
                }
            }
            catch { /* Cache yazma hatası yutulabilir */ }
        }

        public async Task RemoveAsync(string key)
        {
            if (!_isAvailable || _cacheDb == null) return;

            try
            {
                await _cacheDb.KeyDeleteAsync(key);
            }
            catch { /* Silme hatası yutulabilir */ }
        }
    }
}