using System;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache
{
    public class ExpiringCache<TKey, TValue>
        : IDisposable
    {
	    private readonly object _locker = new object();

        private readonly MemoryCache _cache;

        private readonly Func<TKey, TValue> _valueProvider;
        private readonly int _cacheExpirationMinutes;

        public ExpiringCache(
            Func<TKey, TValue> valueProvider,
            int cacheExpirationMinutes)
        {
            _valueProvider = valueProvider;
            _cacheExpirationMinutes = cacheExpirationMinutes;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public TValue Get(TKey key)
        {
            TValue value;

            var exist = _cache.TryGetValue(key.ToString(), out var objValue);
            if (exist) return (TValue)objValue;

		    lock (_locker)
		    {
			    exist = _cache.TryGetValue(key.ToString(), out objValue);
                if (exist) return (TValue)objValue;

                value = _valueProvider(key);
                
                _cache.Set(key, value, new MemoryCacheEntryOptions
		        {
			        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(_cacheExpirationMinutes))
		        });
		    }

		    return value;
		}
        
		public void Remove(TKey key)
		{
			_cache.Remove(key.ToString());
		}

		public void Dispose()
		{
			_cache.Dispose();
		}
	}
}