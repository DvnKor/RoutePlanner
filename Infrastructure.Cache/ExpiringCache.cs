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
        private readonly TimeSpan _absoluteExpirationRelativeToNow;

        public ExpiringCache(
            Func<TKey, TValue> valueProvider,
            TimeSpan absoluteExpirationRelativeToNow)
        {
            _valueProvider = valueProvider;
            _absoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public TValue Get(TKey key)
        {
            TValue value;

            var exist = _cache.TryGetValue(key.ToString(), out var objValue);
            if (exist)
            {
	            return (TValue)objValue;
            }

		    lock (_locker)
		    {
			    exist = _cache.TryGetValue(key.ToString(), out objValue);
			    if (exist)
			    {
				    return (TValue)objValue;
			    }

                value = _valueProvider(key);

                _cache.Set(key.ToString(), value, new MemoryCacheEntryOptions
		        {
			        AbsoluteExpirationRelativeToNow = _absoluteExpirationRelativeToNow,
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